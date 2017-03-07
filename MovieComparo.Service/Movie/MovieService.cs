using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieComparo.ApiClients.Factories;
using MovieComparo.Models.Movie;
using MovieComparo.Config;
using MovieComparo.Service.Helpers;

namespace MovieComparo.Service.Movie
{
    public class MovieService : IMovieService
    {
        private readonly IMovieProviderApiClientFactory _apiClientProviderFactory;
        private IConfig _config;
        private readonly ICacheService _cacheService;

        #region Constructors

        public MovieService() : this(new Config.Config())
        {}

        // Could use an IOC Container to clean this up.
        public MovieService(IConfig config)
            : this(new MovieProviderApiClientFactory(config),
                   config,
                   new CacheService())
        {}

        // Should only be called for mocking tests.
        public MovieService(IMovieProviderApiClientFactory apiClientProviderFactory, IConfig config, ICacheService cacheService)
        {
            _apiClientProviderFactory = apiClientProviderFactory;
            _config = config;
            _cacheService = cacheService;
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve movies from available 3rd party API's
        /// </summary>
        /// <param name="term">A term that the movie title starts with</param>
        /// <returns>List of movies</returns>
        public List<MovieSummary> GetMovies(string term)
        {
            var movieProviderApiClients = _apiClientProviderFactory.CreateAll();
            var movies = new ConcurrentBag<MovieSummary>();
            var summaryModelName = nameof(MovieSummary);

            Parallel.ForEach(movieProviderApiClients, providerApiClient =>
            {
                var providerMovieHeader = _cacheService.GetOrSet<MovieHeader>(summaryModelName, providerApiClient.Provider.ToString(), () => providerApiClient.GetSummary());

                if (providerMovieHeader != null && providerMovieHeader.Movies.Any())
                { 
                    var movieSelection = providerMovieHeader.Movies.Where(m => m.Title.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
                    foreach (var movie in movieSelection)
                    {
                        movies.Add(movie);
                    }
                }
            });

            return movies.ToList();
        }

        public List<MoviePriceInfo> GetMoviePrices(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            var movieProviderApiClients = _apiClientProviderFactory.CreateAll();
            var moviePrices = new ConcurrentBag<MoviePriceInfo>();
            var detailModelName = nameof(MovieDetail);

            var movies = GetMovies(title).AsReadOnly();

            Parallel.ForEach(movieProviderApiClients, providerApiClient =>
            {
                foreach (var movie in movies)
                {
                    var movieDetail = _cacheService.GetOrSet<MovieDetail>(detailModelName, movie.ID,
                            () => providerApiClient.GetDetail(movie.ID));

                    if (movieDetail != null)
                    {
                        moviePrices.Add(new MoviePriceInfo(movieDetail));
                    }
                }
            });

            return moviePrices.ToList();
        }
    }
}
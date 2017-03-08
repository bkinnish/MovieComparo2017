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
        private readonly IConfig _config;
        private readonly ICacheService _cacheService;
        private readonly Retry _retry;

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
            _retry = new Retry();
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

            try
            {
                Parallel.ForEach(movieProviderApiClients, apiClient =>
                {

                    try
                    {
                        var providerMovieHeader = _cacheService.Get<MovieHeader>(summaryModelName,
                                apiClient.Provider.ToString(), 
                                () => _retry.Run<MovieHeader>(apiClient.GetSummary, TimeSpan.FromSeconds(1), _config.ApiMaxRetries));

                        if (providerMovieHeader != null && providerMovieHeader.Movies.Any())
                        { 
                            var movieSelection = providerMovieHeader.Movies.Where(m => m.Title.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
                            foreach (var movie in movieSelection)
                            {
                                movies.Add(movie);
                            }
                        }
                    }
                    catch (AggregateException ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the error somewhere and continue. Some data may be available.
                Console.WriteLine(ex.ToString());
            }

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

            try
            {
                Parallel.ForEach(movieProviderApiClients, apiClient =>
                {
                    foreach (var movie in movies.Where(m => m.Provider == apiClient.Provider))
                    {
                        try
                        {
                            var movieId = movie.ID;
                            var movieDetail = _cacheService.Get<MovieDetail>(detailModelName, movieId,
                                    () => _retry.Run<MovieDetail>(() => apiClient.GetDetail(movieId), 
                                                    TimeSpan.FromSeconds(1), _config.ApiMaxRetries));

                            if (movieDetail != null)
                            {
                                moviePrices.Add(new MoviePriceInfo(movieDetail));
                            }
                        }
                        catch (AggregateException ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the error somewhere and continue. Some data may be available.
                Console.WriteLine(ex.ToString());
            }

            return moviePrices.ToList();
        }
    }
}
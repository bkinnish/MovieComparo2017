using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MovieComparo.ApiClients.Movie;
using MovieComparo.Models.Movie;
using MovieComparo.Config;

namespace MovieComparo.Service.Movie
{
    public class MovieService : IMovieService
    {
        private readonly IMovieApiClient _apiClientProvider1;
        private readonly IMovieApiClient _apiClientProvider2;
        private IConfig _config;

        #region Constructors

        public MovieService() : this(new Config.Config())
        {}

        // Could use an IOC Container to clean this up.
        public MovieService(IConfig config)
            : this(new MovieApiClient(config.ApiAddress, config.AccessToken, config.Provider1Name, config),
                   new MovieApiClient(config.ApiAddress, config.AccessToken, config.Provider2Name, config), 
                   config)
        {}

        // Should only be called for mocking tests.
        public MovieService(IMovieApiClient apiClientProvider1, IMovieApiClient apiClientProvider2, IConfig config)
        {
            _apiClientProvider1 = apiClientProvider1;
            _apiClientProvider2 = apiClientProvider2;
            _config = config;
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve movies from available 3rd party API's
        /// </summary>
        /// <param name="term">A term that the movie title starts with</param>
        /// <returns>List of movies</returns>
        public List<Models.Movie.Movie> GetMovies(string term)
        {
            Task<MovieHeader> provider1Task = null;
            Task<MovieHeader> provider2Task = null;

            try
            {
                provider1Task = _apiClientProvider1.GetSummary();
                provider2Task = _apiClientProvider2.GetSummary();
                Task.WaitAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            List<Models.Movie.Movie> movies = new List<Models.Movie.Movie>();
            if (provider1Task?.Result != null)
            {
                movies = provider1Task.Result.Movies.Where(m => m.Title.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
            }
            if (provider2Task?.Result != null)
            {
                movies.AddRange(provider2Task.Result.Movies.Where(m => m.Title.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList());
            }

            return movies;
        }

        /// <summary>
        /// Retrieve movie detail, includes cost for each provider.
        /// </summary>
        /// <param name="id">A movie id </param>
        /// <returns>Movie details</returns>
        public MovieDetail GetMovieDetails(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Task<MovieDetail> provider1Task = null;
            Task<MovieDetail> provider2Task = null;

            try
            {
                if (id.StartsWith("cw"))
                    provider1Task = _apiClientProvider1.GetDetail(id);
                else
                    provider2Task = _apiClientProvider2.GetDetail(id);
                Task.WaitAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (provider1Task?.Result != null)
            {
                provider1Task.Result.Provider = _config.Provider1Name;
                return provider1Task.Result;
            }
            if (provider2Task?.Result != null)
            {
                provider2Task.Result.Provider = _config.Provider2Name;
                return provider2Task.Result;
            }
            return null;
        }

        public List<MoviePriceInfo> GetMoviePrices(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }
            var moviePrices = new List<MoviePriceInfo>();

            var movies = GetMovies(title);
            foreach (var movie in movies)
            {
                var movieDetail = GetMovieDetails(movie.ID);
                if (movieDetail != null)
                {
                    moviePrices.Add(new MoviePriceInfo(movieDetail));
                }
            }
            return moviePrices;
        }
    }
}
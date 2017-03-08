using System.Collections.Generic;
using MovieComparo.ApiClients.Movie;
using MovieComparo.Config;
using MovieComparo.Models;

namespace MovieComparo.ApiClients.Factories
{
    /// <summary>
    /// Create Api Clients that can talk to the different Movie Providers.
    /// </summary>
    public class MovieProviderApiClientFactory : IMovieProviderApiClientFactory
    {
        private readonly IConfig _config;

        public MovieProviderApiClientFactory(IConfig config)
        {
            _config = config;
        }

        public IMovieApiClient Create(MovieProvider provider)
        {
            MovieApiClient apiClient;
            switch (provider)
            {
                case MovieProvider.cinemaworld:
                {
                    apiClient = new MovieApiClient(_config.ApiAddress, _config.AccessToken, MovieProvider.cinemaworld);
                    break;
                }
                case MovieProvider.filmworld:
                {
                    apiClient = new MovieApiClient(_config.ApiAddress, _config.AccessToken, MovieProvider.filmworld);
                    break;
                }
                default:
                {
                    apiClient = default(MovieApiClient);
                    break;
                }
            }
            return apiClient;
        }
        
        public List<IMovieApiClient> CreateAll()
        {
            var providers = new List<IMovieApiClient>
            {
                Create(MovieProvider.cinemaworld),
                Create(MovieProvider.filmworld)
            };
            return providers;
        }
    }
}
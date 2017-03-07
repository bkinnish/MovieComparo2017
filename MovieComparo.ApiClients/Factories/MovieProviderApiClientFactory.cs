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
        private IConfig _config;

        public MovieProviderApiClientFactory(IConfig config)
        {
            _config = config;
        }

        public MovieApiClient Create(MovieProvider provider)
        {
            MovieApiClient apiClient;
            switch (provider)
            {
                case MovieProvider.cinemaworld:
                {
                    apiClient = new MovieApiClient(_config.ApiAddress, _config.AccessToken, MovieProvider.cinemaworld, _config);
                    break;
                }
                case MovieProvider.filmworld:
                {
                    apiClient = new MovieApiClient(_config.ApiAddress, _config.AccessToken, MovieProvider.filmworld, _config);
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
        
        public List<MovieApiClient> CreateAll()
        {
            var providers = new List<MovieApiClient>();
            providers.Add(Create(MovieProvider.cinemaworld));
            providers.Add(Create(MovieProvider.filmworld));
            return providers;
        }
    }
}
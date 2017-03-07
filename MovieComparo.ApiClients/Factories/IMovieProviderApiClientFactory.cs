using System;
using System.Collections.Generic;
using MovieComparo.ApiClients.Movie;
using MovieComparo.Models;

namespace MovieComparo.ApiClients.Factories
{
    public interface IMovieProviderApiClientFactory
    {
        MovieApiClient Create(MovieProvider provider);
        List<MovieApiClient> CreateAll();
    }
}

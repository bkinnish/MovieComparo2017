using System;
using System.Collections.Generic;
using MovieComparo.Models.Movie;

namespace MovieComparo.Service.Movie
{
    public interface IMovieService
    {
        List<MovieSummary> GetMovies(string searchTitle);
        List<MoviePriceInfo> GetMoviePrices(string title);
    }
}

using System.Collections.Generic;
using MovieComparo.Models.Movie;

namespace MovieComparo.Service.Movie
{
    public interface IMovieService
    {
        List<Models.Movie.Movie> GetMovies(string searchTitle);
        MovieDetail GetMovieDetails(string id);
        List<MoviePriceInfo> GetMoviePrices(string title);
    }
}

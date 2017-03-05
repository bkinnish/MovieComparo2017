using System.Collections.Generic;
using MovieComparo.Models.Movie;

namespace MovieComparo.Service.Movie
{
    public interface IMovieService
    {
        List<Models.Movie.Movie> GetMovies(string searchTitle);
        List<MovieDetail> GetMovieDetails(string id);
        List<MoviePriceInfo> GetMoviePrices(string id);
    }
}

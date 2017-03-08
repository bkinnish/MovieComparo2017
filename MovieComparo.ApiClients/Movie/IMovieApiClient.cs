using MovieComparo.Models;
using MovieComparo.Models.Movie;

namespace MovieComparo.ApiClients.Movie
{
    public interface IMovieApiClient
    {
        MovieProvider Provider { get; }
        MovieHeader GetSummary();
        MovieDetail GetDetail(string id);
    }
}

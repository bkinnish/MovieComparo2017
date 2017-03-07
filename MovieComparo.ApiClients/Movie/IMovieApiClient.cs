using MovieComparo.Models.Movie;

namespace MovieComparo.ApiClients.Movie
{
    public interface IMovieApiClient
    {
        MovieHeader GetSummary();
        MovieDetail GetDetail(string id);
    }
}

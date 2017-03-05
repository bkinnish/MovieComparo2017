
using System.Threading.Tasks;
using MovieComparo.Models.Movie;

namespace MovieComparo.ApiClients.Movie
{
    public interface IMovieApiClient
    {
        Task<MovieHeader> GetSummary();
        Task<MovieDetail> GetDetail(string id);
    }
}

using System.Collections.ObjectModel;

namespace MovieComparo.Models.Movie
{
    public class MovieHeader
    {
        public Collection<MovieSummary> Movies { get; set; }
    }

    /// <summary>
    /// http://webjetapitest.azurewebsites.net/api/cinemaworld/movies
    /// </summary>
    public class MovieSummary : IMovieProvider
    {
        // eg. "Star Wars: Episode IV - A New Hope",
        public string Title { get; set; }

        // eg. "1977"
        public string Year { get; set; }

        // eg "cw0076759"
        public string ID { get; set; }

        // eg. "movie",
        public string Type { get; set; }

        // eg. "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
        public string Poster { get; set; }

        // eg. "cinemaworld" or "filmworld"
        public MovieProvider Provider { get; set; }
    }
}

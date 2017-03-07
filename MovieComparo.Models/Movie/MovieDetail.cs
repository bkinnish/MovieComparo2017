namespace MovieComparo.Models.Movie
{
    /// <summary>
    /// http://webjetapitest.azurewebsites.net/api/cinemaworld/movie/cw0076759
    /// </summary>
    public class MovieDetail : IMovieProvider
    {
        // eg. "Title": "Star Wars: Episode IV - A New Hope"
        public string Title { get; set; }
        // eg. "Year": "1977"
        public string Year { get; set; }
        // eg. "Rated": "PG"
        public string Rated { get; set; }
        // eg. "Released": "25 May 1977"
        public string Released { get; set; }
        // eg. "Runtime": "121 min"
        public string Runtime { get; set; }
        // eg. "Genre": "Action, Adventure, Fantasy"
        public string Genre { get; set; }
        // eg. "Director": "George Lucas"
        public string Director { get; set; }
        // eg. "Writer": "George Lucas"
        public string Writer { get; set; }
        // eg. "Actors": "Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing"
        public string Actors { get; set; }
        // eg. "Plot": "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a wookiee and two droids to save the galaxy from the Empire's world-destroying battle-station, while also attempting to rescue Princess Leia from the evil Darth Vader."
        public string Plot { get; set; }
        // eg. "Language": "English"
        public string Language { get; set; }
        // eg. "Country": "USA"
        public string Country { get; set; }
        // eg. "Awards": "Won 6 Oscars. Another 48 wins & 28 nominations."
        public string Awards { get; set; }
        // eg. "Poster": "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
        public string Poster { get; set; }
        // eg. "Metascore": "92"
        public string Metascore { get; set; }
        // eg. "Rating": "8.7"
        public string Rating { get; set; }
        // eg. "Votes": "915,459"
        public string Votes { get; set; }
        // eg. "ID": "cw0076759"
        public string ID { get; set; }
        // eg. "Type": "movie"
        public string Type { get; set; }
        // eg. "Price": "123.5"
        public string Price { get; set; }

        // eg. "cinemaworld" or "filmworld"
        public MovieProvider Provider { get; set; }
    }
}
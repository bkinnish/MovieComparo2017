using Newtonsoft.Json;

namespace MovieComparo.Models
{
    public class MovieSummaryMovieSummary
    {
        // eg. "Star Wars: Episode IV - A New Hope",
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        // eg "cw0076759"
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        // eg. "1977"
        [JsonProperty(PropertyName = "year")]
        public string Year { get; set; }

        // eg. "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
        [JsonProperty(PropertyName = "poster")]
        public string Poster { get; set; }
    }
}
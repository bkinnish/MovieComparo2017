using Newtonsoft.Json;

namespace MovieComparo.Models.Movie
{
    public class MoviePriceInfo
    {
        public MoviePriceInfo(MovieDetail movieDetail)
        {
            Provider = movieDetail.Provider.ToString();
            ID = movieDetail.ID;
            Price = movieDetail.Price;
        }
        // eg. "cinemaworld" or "filmworld"
        [JsonProperty(PropertyName = "provider")]
        public string Provider { get; set; }

        // eg. "cw0076759"
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        // eg. "123.5"
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }
    }
}
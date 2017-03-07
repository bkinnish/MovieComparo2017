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
        public string Provider { get; set; }
        // eg. "cw0076759"
        public string ID { get; set; }
        // eg. "123.5"
        public string Price { get; set; }
    }
}
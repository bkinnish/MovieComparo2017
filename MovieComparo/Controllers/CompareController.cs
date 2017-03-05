using System;
using System.Linq;
using System.Web.Mvc;
using MovieComparo.Service.Movie;
using Newtonsoft.Json;

namespace MovieComparo.Controllers
{
    public class CompareController : Controller
    {
        private readonly IMovieService _movieService;

        #region contructors

        public CompareController() 
            : this(new MovieService())
        {}

        public CompareController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        #endregion contructors

        // Display the compare movies page
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get a list of movies to display in the autocomplete search box.
        /// </summary>
        /// <param name="term">Part of movie title to search for</param>
        /// <returns>Movie summary as Json</returns>
        public ActionResult GetMoviesSummary(string term)
        {
            var movieNames = _movieService.GetMovies(term).Select(movie =>
                new {
                    title = movie.Title,
                    id = movie.ID,
                    year = movie.Year,
                    poster = movie.Poster
                }).ToList();
            var movieJson = JsonConvert.SerializeObject(movieNames);

            return Json(movieJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get multiple prices from multiple providers for a single movie.
        /// </summary>
        /// <param name="id">The movie id (PLU)</param>
        /// <returns>Movie summary as Json</returns>
        public ActionResult GetMoviePrices(string id)
        {
            var moviePrices = _movieService.GetMoviePrices(id).Select(movie =>
                new {
                    provider = movie.Provider,
                    id = movie.ID,
                    price = movie.Price
                }).ToList();
            var movieJson = JsonConvert.SerializeObject(moviePrices);

            return Json(movieJson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetMovieDetails(string id)
        //{
        //    var movieDetails = _movieService.GetMovieDetails(id);
        //    return Json(movieDetails, JsonRequestBehavior.AllowGet);
        //}
    }
}
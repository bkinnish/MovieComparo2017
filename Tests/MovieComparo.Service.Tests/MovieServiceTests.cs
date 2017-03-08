using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MovieComparo.ApiClients.Factories;
using MovieComparo.ApiClients.Movie;
using MovieComparo.Models;
using MovieComparo.Models.Movie;
using MovieComparo.Service.Helpers;
using MovieComparo.Service.Movie;
using NSubstitute;
using Xunit;

namespace MovieComparo.Service.Tests
{
    public class MovieServiceTests
    {
        readonly IMovieService _service = new MovieService();

        #region Integration Tests

        [Fact]
        public void GetMovies_GetForAllProvides_ReturnAvailableMovies()
        {
            // Arrange
            var retry = new Retry();
            var title = "Star Wars: Episode IV - A New Hope";

            // Act
            var result = retry.Run(() => _service.GetMovies(title), TimeSpan.FromSeconds(1), 4);

            // Assert
            Assert.NotNull(result);

            // This is a flakey integration test that can fail or pass!
            Assert.True(result.Any());
        }

        [Fact]
        public void GetMovieDetails_PassMovieName_ReturnMovieDetails()
        {
            // Arrange
            var retry = new Retry();
            var title = "Star Wars: Episode IV - A New Hope";

            // Act
            var result = retry.Run(() => _service.GetMoviePrices(title), TimeSpan.FromSeconds(1), 4);

            // Assert
            Assert.NotNull(result);

            // This is a flakey integration test that can fail or pass!
            Assert.True(result.Any());
        }

        #endregion

        #region Unit Tests

        [Fact]
        public void GetMovies_PassMovieName_ReturnMockMovieDetails()
        {
            // Arrange
            var title = "Star Wars: Episode IV - A New Hope";

            var movieHeaderData = new MovieHeader()
            {
                Movies = new Collection<MovieSummary>()
                {
                    new MovieSummary()
                    {
                        ID = "cw12345",
                        Title = title,
                        Provider = MovieProvider.cinemaworld,
                        Year = "1234",
                        Type = "movie"
                    }
                }
            };

            var providerFactory = Substitute.For<IMovieProviderApiClientFactory>();
            var provider = Substitute.For<IMovieApiClient>();
            provider.GetSummary().Returns(movieHeaderData);
            providerFactory.CreateAll().Returns(new List<IMovieApiClient>() { provider } );

            var movieService = new MovieService(providerFactory, new Config.Config(), new CacheService());

            // Act
            var result = movieService.GetMovies(title);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.Equal(title, result.First().Title);
        }

        #endregion
    }
}

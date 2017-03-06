using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MovieComparo.Service.Movie;
using Xunit;

namespace MovieComparo.Service.Tests
{
    public class MovieServiceTests
    {
        readonly IMovieService _service = new MovieService();

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
            Assert.True(result.Any());
        }
    }


    public class Retry
    {
        public T Run<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 5)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(retryInterval);
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}

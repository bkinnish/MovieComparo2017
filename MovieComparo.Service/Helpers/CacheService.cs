using System;
using System.Runtime.Caching;

namespace MovieComparo.Service.Helpers
{
    /// <summary>
    /// Cache data in the .NET MemoryCache.
    /// http://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application
    /// Example: var movieDetail = _cacheService.GetOrSet&ltMovieDetail&gt(detailModelName, movie.ID,
    ///                       () => providerApiClient.GetDetail(movie.ID));
    /// </summary>
    public class CacheService : ICacheService
    {
        /// <summary>
        /// Get data from the cache or service.
        /// </summary>
        /// <typeparam name="T">Data to be loaded</typeparam>
        /// <param name="modelName">Model Name (ie nameof(MovieSummary))</param>
        /// <param name="cacheKey">unique identifier (ie id)</param>
        /// <param name="service">The service to lookup data if it is not in the cache.</param>
        /// <returns>Data that has been loaded</returns>
        public T GetOrSet<T>(string modelName, string cacheKey, Func<T> service) where T : class
        {
            var combinedCacheKey = modelName + cacheKey;
            T item = MemoryCache.Default.Get(combinedCacheKey) as T;
            if (item == null)
            {
                // TODO: Could do with a thread lock to control multiple updates at once on multiple threads.
                item = service();
                if (item != null)
                {
                    MemoryCache.Default.Add(combinedCacheKey, item, DateTime.Now.AddMinutes(10));
                }
            }
            return item;
        }
    }
}

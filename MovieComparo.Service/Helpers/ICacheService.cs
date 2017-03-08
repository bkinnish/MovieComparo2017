using System;
using System.Runtime.Caching;

namespace MovieComparo.Service.Helpers
{
    public interface ICacheService
    {
        T Get<T>(string cacheKey, string id, Func<T> service, CacheItemPolicy cachePolicy = null) where T : class;
    }
}

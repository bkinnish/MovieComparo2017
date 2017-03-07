using System;

namespace MovieComparo.Service.Helpers
{
    public interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, string id, Func<T> service) where T : class;
    }
}

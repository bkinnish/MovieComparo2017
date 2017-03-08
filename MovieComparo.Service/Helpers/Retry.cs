using System;
using System.Collections.Generic;
using System.Threading;

namespace MovieComparo.Service.Helpers
{
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

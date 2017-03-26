using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MovieComparo.ApiClients.Base
{
    public class ApiClientBase : IDisposable
    {
        protected HttpClient Client;

        public ApiClientBase(string apiAddress, string accessToken)
        {
            if (String.IsNullOrWhiteSpace(apiAddress))
            {
                throw new ArgumentNullException(nameof(apiAddress));
            }
            if (String.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            Client = new HttpClient();
            Client.Timeout = TimeSpan.FromMilliseconds(1000);
            Client.BaseAddress = new Uri(apiAddress);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("x-access-token", accessToken);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}

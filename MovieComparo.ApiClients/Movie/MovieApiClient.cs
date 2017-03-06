using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using MovieComparo.ApiClients.Base;
using MovieComparo.Config;
using MovieComparo.Models.Movie;
using Newtonsoft.Json;

namespace MovieComparo.ApiClients.Movie
{
    public class MovieApiClient : ApiClientBase, IMovieApiClient
    {
        private readonly string _provider;
        private readonly IConfig _config;

        /// <summary>
        /// Access data from a web service.
        /// </summary>
        /// <param name="apiAddress">http://webjetapitest.azurewebsites.net/
        /// </param>
        /// <param name="accessToken">eg. sjd1HfkjU83ksdsm3802k</param>
        /// <param name="provider">cinemaworld or filmworld</param>
        /// <param name="config">Movie Comparo configuration settings</param>
        public MovieApiClient(string apiAddress, string accessToken, string provider, IConfig config) : base(apiAddress, accessToken)
        {
            _provider = provider;
            _config = config;
        }

        public Task<MovieHeader> GetSummary()
        {
            for (int i = 0; i < _config.ApiMaxRetries; i++)
            {
                try
                {
                    using (HttpResponseMessage response = Client.GetAsync($"api/{_provider}/movies").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                return Task.FromResult(content.ReadAsAsync<MovieHeader>().Result);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return Task.FromResult(default(MovieHeader));
        }

        public Task<MovieDetail> GetDetail(string id)
        {
            for (int i = 0; i < _config.ApiMaxRetries; i++)
            {
                try
                {
                    using (HttpResponseMessage response = Client.GetAsync($"api/{_provider}/movie/{id}").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                // http://stackoverflow.com/questions/12754463/json-net-ignore-property-during-deserialization
                                var jsonResponseString = response.Content.ReadAsStringAsync();
                                jsonResponseString.Wait();
                                var jsonSerializerSettings = new JsonSerializerSettings
                                {
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };
                                var jsonObject = JsonConvert.DeserializeObject<MovieDetail>(jsonResponseString.Result, jsonSerializerSettings);

                                return Task.FromResult(jsonObject);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return Task.FromResult(default(MovieDetail));
        }

    }
}

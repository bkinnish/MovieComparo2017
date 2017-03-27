using System;
using System.Net.Http;
using MovieComparo.ApiClients.Base;
using MovieComparo.Models;
using MovieComparo.Models.Movie;
using Newtonsoft.Json;

namespace MovieComparo.ApiClients.Movie
{
    public class MovieApiClient : ApiClientBase, IMovieApiClient
    {
        public MovieProvider Provider { get; internal set; }

        /// <summary>
        /// Access data from a web service.
        /// </summary>
        /// <param name="apiAddress">http://webjetapitest.azurewebsites.net/
        /// </param>
        /// <param name="accessToken">eg. sjd1HfkjU83ksdsm3802k</param>
        /// <param name="provider">cinemaworld or filmworld</param>
        public MovieApiClient(string apiAddress, string accessToken, MovieProvider provider) : base(apiAddress, accessToken)
        {
            Provider = provider;
        }

        public MovieHeader GetSummary()
        {
            using (HttpResponseMessage response = Client.GetAsync($"api/{Provider}/movies").Result)
            {
                var jsonObject = ParseResponseContent<MovieHeader>(response);
                foreach (var movie in jsonObject.Movies)
                {
                    movie.Provider = Provider;
                }
                return jsonObject;
            }
        }

        public MovieDetail GetDetail(string id)
        {
            using (HttpResponseMessage response = Client.GetAsync($"api/{Provider}/movie/{id}").Result)
            {
                var jsonObject = ParseResponseContent<MovieDetail>(response);
                jsonObject.Provider = Provider;
                return jsonObject;
            }
        }

        private T ParseResponseContent<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            using (HttpContent content = response.Content)
            {
                var jsonResponseString = content.ReadAsStringAsync();
                jsonResponseString.Wait();
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var jsonObject = JsonConvert.DeserializeObject<T>(jsonResponseString.Result, jsonSerializerSettings);
                return jsonObject;
            }
        }

    }
}
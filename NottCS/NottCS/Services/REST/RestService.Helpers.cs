﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace NottCS.Services.REST
{
    //TODO: Write unit test for REST
    internal static partial class RestService
    {
        //TODO: Update the Uri when the domain name is available
        /// <summary>
        /// Base address where the API endpoints are stored
        /// </summary>
        private const string BaseAddress = "https://testing-endpoints.herokuapp.com/";

        //TODO: Setup client with proper headers
        /// <summary>
        /// Client with setup. To add Bearer Authorization, intitialize client with SetupClient(string accessToken)
        /// </summary>
        private static readonly HttpClient Client = new HttpClient()
        {
            BaseAddress = new Uri(BaseAddress),
            Timeout = new TimeSpan(0, 0, 10)
        };

        /// <summary>
        /// Setups the Client with authorization header
        /// </summary>
        /// <param name="accessToken"></param>
        public static void SetupClient(string accessToken)
        {
            DebugService.WriteLine("HttpClient is setting up...");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        //TODO: DEAL WITH a
        public static void IsValidToken()
        {
            var a = App.MicrosoftAuthenticationResult.ExpiresOn;
        }

        /// <summary>
        /// Resets the authorization header
        /// </summary>
        public static void ResetClient()
        {
            Client.DefaultRequestHeaders.Clear();
        }

        private static HttpRequestMessage HttpRequestMessageGenerator(HttpMethod httpMethod, string requestUri, object requestBody = null)
        {
            #region ObjectValidator

            if (httpMethod == HttpMethod.Post && requestBody == null)
            {
                DebugService.WriteLine("[BaseRestService] WARNING : No valid request body");
            }

            #endregion

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequest;
            if (httpMethod == HttpMethod.Get)
            {
                httpRequest = new HttpRequestMessage(httpMethod, requestUri);
            }
            else
            {
                httpRequest = new HttpRequestMessage(httpMethod, requestUri)
                {
                    Content = content
                };
            }
            return httpRequest;
        }

        /// <summary>
        /// Generates proper request Uri based on the method
        /// </summary>
        /// <typeparam name="T">Type of modal class requested</typeparam>
        /// <param name="httpMethod">Request HttpMethod</param>
        /// <param name="identifier">Identifier for the server to lookup data</param>
        /// <returns></returns>
        private static string UriGenerator<T>(HttpMethod httpMethod, string identifier = null)
        {
            var returnUri = BaseAddress + "/azuread-user/me/";

            //TODO: Write a Uri generator logic based on the REST endpoint

            return returnUri;
        }

    }
}

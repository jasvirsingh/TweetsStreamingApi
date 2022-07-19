using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TweetsAccessApi.Infrastructure.Exceptions;
using TwitterAccessApi.Integration.Interfaces;

namespace TwitterAccessApi.Integration
{
    /// <summary>
    ///     Rest API clients for calling DocuSign API's
    /// </summary>
    public class RestApiClient : IRestApiClient
    {
        public RestApiClient()
        {

        }

        /// <summary>
        ///     Prepares rest request for Docusign based on parameters
        /// </summary>
        /// <returns>
        ///  <see cref="RestRequest"/>
        /// </returns>
        public RestRequest PrepareRequest(string path, Method method, Object postBody, Dictionary<string, string> headerParams, string contentType)
        {
            var request = new RestRequest(path, method);
            foreach (var param in headerParams)
                request.AddHeader(param.Key, param.Value);
            if (postBody != null)
            {
                request.AddJsonBody(postBody);
            }
            return request;
        }

        /// <summary>
        ///     Performs Rest API post call
        /// </summary>
        /// <param name="apiUrl">
        /// </param>
        /// <param name="methodType">
        ///     <see cref="Method"/>
        /// </param>
        /// <param name="body">
        /// </param>
        /// <param name="token">
        /// </param>
        /// <param name="contentType">
        /// </param>
        /// <returns>
        ///   <see cref="Task"/> of <see cref="IRestResponse"/>
        /// </returns>
        public async Task<IRestResponse> PerformApiCallAsync(string apiUrl, Method methodType, object body, string token, string contentType)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", contentType);
            headers.Add("Authorization", "Bearer " + token);
            var requestbject = PrepareRequest(apiUrl, methodType, body, headers, contentType);
            var client = new RestClient(apiUrl);
            try
            {
                var tempResponseJSON = await client.ExecuteAsync(requestbject);
                return tempResponseJSON;

            }
            catch (Exception error)
            {
                throw new TwitterApiException(error.InnerException.ToString());
            }
        }

        /// <summary>
        ///     Deserialize api response
        /// </summary>
        /// <param name="content">
        ///<see cref="byte[]"/>
        /// </param>
        /// <param name="type">
        ///     <see cref="Type"/>
        /// </param>
        /// <returns>
        ///   <see cref="object"/>
        /// </returns>
        public object Deserialize(byte[] content, Type type)
        {
            if (type == typeof(Stream))
            {
                MemoryStream ms = new MemoryStream(content);
                return ms;
            }

            throw new TwitterApiException("Unhandled response type.");
        }

    }
}

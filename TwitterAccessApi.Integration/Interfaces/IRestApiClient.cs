using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterAccessApi.Integration.Interfaces
{
    /// <summary>
    ///     Interface for the Twitter API
    /// </summary>
    public interface IRestApiClient
    {
        /// <summary>
        ///     Prepares rest request for twitter call based on parameters
        /// </summary>
        /// <returns>
        ///  <see cref="RestRequest"/>
        /// </returns>
        RestRequest PrepareRequest(string path, Method method, Object postBody, Dictionary<string, string> headerParams, string contentType);

        /// <summary>
        ///     Performs Rest API post call for Twitter
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
        Task<IRestResponse> PerformApiCallAsync(string apiUrl, Method methodType, object body, string token, string contentType);

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
        object Deserialize(byte[] content, Type type);
    }
}

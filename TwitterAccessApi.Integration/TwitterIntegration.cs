using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using TweetsAccessApi.Model;
using TwitterAccessApi.Integration.Interfaces;
using TwitterAccessApi.Integration.Models;

namespace TwitterAccessApi.Integration
{
    /// <summary>
    ///     Implementation for the <see cref="ITwitterIntegration"/>
    /// </summary>
    public class TwitterIntegration : ITwitterIntegration
    {
        private readonly IRestApiClient _restApiClient;
        private readonly IOptions<ApiConfigModel> _apiConfig;

        /// <summary>
        ///     Constructor for the EnvelopeIntegration <see cref="EnvelopeIntegration"/>
        /// </summary>
        /// <param name="restApiClient">
        ///     <see cref="RestApiClient"/> of <see cref="RestApiClient"/>
        /// </param>
        /// <param name="apiConfig">
        ///     <see cref="ApiConfigModel"/>
        /// </param>
        public TwitterIntegration(IRestApiClient restApiClient, IOptions<ApiConfigModel> apiConfig)
        {
            _restApiClient = restApiClient ?? throw new MissingMemberException($" {nameof(restApiClient)} is a required dependency.");
            _apiConfig = apiConfig ?? throw new MissingMemberException($" {nameof(apiConfig)} is a required dependency.");
        }

        /// <summary>
        ///     Parses the <see cref="EnvelopeSummary"/>
        ///     Returns the DocuSign envelope detail if we got back an OK, otherwise, check the status and throw the correcft exception
        /// </summary>
        /// <param name="envelopeId">
        /// </param>
        /// <param name="token">
        /// </param>
        /// <returns>
        ///     <see cref="Task"/> of <see cref="Envelope"/>
        /// </returns>
        public async Task<Tweets> GetTweetsSteamAsync()
        {
            var result = await _restApiClient.PerformApiCallAsync(_apiConfig.Value.ApiUrl, Method.GET, null, _apiConfig.Value.BearerToken, "application/json");
            
            var tweets = JsonConvert.DeserializeObject<Tweets>(result.Content);

            if(tweets.Data == null)
            {
                tweets.Message = result.Content;
            }
            return tweets;
        }
    }
}

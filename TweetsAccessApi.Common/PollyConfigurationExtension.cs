using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using RestSharp;
using System;
using System.Net;

namespace TweetsAccessApi.Common
{
    /// <summary>
    ///     Polly configuration extnsion
    /// </summary>
    public static class PollyConfigurationExtension
    {
        private const int DEFAULT_WAIT_RETRY_SECONDS = 5;
        private const int DEFAULT_RETRY_COUNT = 3;

        /// <summary>
        ///     No Operation Policy Name
        /// </summary>
        public const string NO_OP_POLICY_NAME = "NoOpPolicy";

        /// <summary>
        ///     Zero Status Policy Name
        /// </summary>
        public const string ZERO_STATUS_RETRY_POLICY_NAME = "ZeroStatusRetryPolicy";

        /// <summary>
        ///     Not Found Policy Name
        ///     This policy should only be used by the Wildfire Get Report call
        /// </summary>
        public const string NOT_FOUND_RETRY_POLICY_NAME = "NotFoundRetryPolicy";

        /// <summary>
        ///     Request Timed Out Policy Name
        /// </summary>
        public const string REQUEST_TIMEOUT_RETRY_POLICY_NAME = "RequestTimeOutRetryPolicy";

        /// <summary>
        ///     Internal Server Error Policy Name
        /// </summary>
        public const string INTERNAL_SERVER_ERROR_RETRY_POLICY_NAME = "InternalServerErrorRetryPolicy";

        /// <summary>
        ///     Adds polly registry to the services collection
        /// </summary>
        /// <param name="services">
        ///     <see cref="IServiceCollection"/>
        /// </param>
        public static void AddPollyConfiguration(this IServiceCollection services)
        {
            PolicyRegistry registry = new PolicyRegistry
            {
                {
                    NO_OP_POLICY_NAME,
                    Policy.NoOpAsync<IRestResponse>()
                },
                {
                    ZERO_STATUS_RETRY_POLICY_NAME,
                    Policy.HandleResult<IRestResponse>(
                        r => r?.StatusCode == 0)
                            .WaitAndRetryAsync(DEFAULT_RETRY_COUNT, retryAttempt =>
                                TimeSpan.FromSeconds(DEFAULT_WAIT_RETRY_SECONDS))
                },
                {
                    NOT_FOUND_RETRY_POLICY_NAME,
                    Policy.HandleResult<IRestResponse>(
                        r => r?.StatusCode == HttpStatusCode.NotFound)
                            .WaitAndRetryAsync(DEFAULT_RETRY_COUNT, retryAttempt =>
                                TimeSpan.FromSeconds(DEFAULT_WAIT_RETRY_SECONDS))
                },
                {
                    REQUEST_TIMEOUT_RETRY_POLICY_NAME,
                    Policy.HandleResult<IRestResponse>(
                        r => r?.StatusCode == HttpStatusCode.RequestTimeout)
                            .WaitAndRetryAsync(DEFAULT_RETRY_COUNT, retryAttempt =>
                                TimeSpan.FromSeconds(DEFAULT_WAIT_RETRY_SECONDS))
                },
                {
                    INTERNAL_SERVER_ERROR_RETRY_POLICY_NAME,
                    Policy.HandleResult<IRestResponse>(
                        r => r?.StatusCode == HttpStatusCode.InternalServerError)
                            .WaitAndRetryAsync(DEFAULT_RETRY_COUNT, retryAttempt =>
                                TimeSpan.FromSeconds(DEFAULT_WAIT_RETRY_SECONDS))
                }
            };

            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TweetsAccessApi.Model.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TweetsAccessApi.Model;
using TweetsAccessApi.Infrastructure;
using System;

namespace TweetsAccessAPI.Controllers
{
    /// <summary>
    ///  Controller for handling Tweets
    /// </summary>
    public class TweetsController : ControllerBase
    {
        private readonly ITweetsStreamingService _tweetsStreamingService;
        private readonly ILogger<TweetsController> _logger;

        /// <summary>
        /// Constructor for <see cref="TweetsController"/>
        /// </summary>
        /// <param name="tweetsStreamingService">
        ///     <see cref="ITweetsStreamingService"/>
        /// </param>
        /// <param name="logger">
        ///     <see cref="ILogger"/>
        /// </param>
        public TweetsController(ITweetsStreamingService tweetsStreamingService, ILogger<TweetsController> logger)
        {
            _tweetsStreamingService = tweetsStreamingService ?? throw new MissingMemberException($"{nameof(tweetsStreamingService)} is a required dependency;");
            _logger = logger ?? throw new MissingMemberException($"{nameof(logger)} is a required dependency;");
        }

        /// <summary>
        /// Gets tweets count and list of top 10 hashtags
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server error</response>
        /// <response code="503">Service unavailable</response>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTweetsInfo")]
        [SwaggerResponse((int)HttpStatusCode.OK, type:typeof(TweetsRs))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorDetails))]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, type: typeof(ErrorDetails))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, type: typeof(ErrorDetails))]
        public async Task<IActionResult> GetTweetsInfo()
        {
            _logger.LogInformation("Receiced GetTweetsInfo request");

            var result =  _tweetsStreamingService.GetTweetsCount();

            return Ok(result);
        }
    }
}

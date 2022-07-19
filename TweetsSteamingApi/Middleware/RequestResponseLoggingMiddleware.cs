using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TweetsAccessApi.Infrastructure;

namespace TweetsSteamingApi.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                IHttpContextAccessor accessor,
                                             ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _accessor = accessor;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Some logic on the request before it being passed on to the next middleware
            var requestResponseLog = new RequestResponseLog
            {
                RequestTime = DateTimeOffset.UtcNow,
                Request = await FormatRequest(context)
            };
            // Passing the request on to the next middleware
            await _next(context);
            // Some logic after all other middlewares have finished and we have the response
            requestResponseLog.ResponseTime = DateTimeOffset.UtcNow;
            requestResponseLog.Response = await FormatResponse(context);
            _logger.LogInformation(requestResponseLog.Request);
        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            var emailClaim = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Email);
            string userEmail = emailClaim != null ? emailClaim.Value : "No user";
            var logMessage = $"IP: {request.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}, " +
                $"Schema: {request.Scheme}, Method: {request.Method}, " +
                $"User: {userEmail}, " +
                $"Host: {request.Host}, " +
                $"Path: {request.Path}";

            return logMessage;
        }

        private async Task<string> FormatResponse(HttpContext context)
        {
            HttpResponse response = context.Response;
           
            var logMessage = $"Status: {response.StatusCode} : Body:{response.Body} ";

            return logMessage;
        }
    }
}

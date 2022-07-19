using System;

namespace TweetsAccessApi.Infrastructure
{
    /// <summary>
    /// Class used to keep request and response information for logging purposes.
    /// </summary>
    public class RequestResponseLog
    {
        public string Request { get; set; }
        public DateTimeOffset RequestTime { get; set; }
        public string Response { get; set; }
        public DateTimeOffset ResponseTime { get; set; }
    }
}

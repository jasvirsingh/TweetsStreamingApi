using System;

namespace TweetsAccessApi.Infrastructure.Exceptions
{
    public class TwitterApiException : Exception
    {
        public TwitterApiException()
        {
        }

        public TwitterApiException(string message) : base(message)
        { }
    }
}

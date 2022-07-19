using System.Collections.Generic;

namespace TwitterAccessApi.Integration.Models
{
    public class Tweets
    {
        public List<TweetInfo> Data { get; set; }
        public string Message { get; set; }
    }
}

using System.Collections.Generic;

namespace TweetsAccessApi.Model
{
    public class TweetsRs
    {
        public int TweetsReceivedCount { get; set; }
        public List<string> Top10HashTags { get; set; }
    }
}

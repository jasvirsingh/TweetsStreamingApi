namespace TwitterAccessApi.Integration.Models
{
    public class TweetUrl
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Url { get; set; }
        public string Expanded_Url { get; set; }
        public string Display_Url { get; set; }
        public TweetImage[] Images { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Unwound_Url { get; set; }
    }
}

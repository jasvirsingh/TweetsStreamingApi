namespace TwitterAccessApi.Integration.Models
{
    public class TweetInfo
    {
        public string Author_Id { get; set; }
        public string Created_At { get; set; }
        public string Id { get; set; }
        public string Lang { get; set; }
        public bool Possibly_Sensitive { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
        public TweetUrl[] Entities { get; set; }
    }
}

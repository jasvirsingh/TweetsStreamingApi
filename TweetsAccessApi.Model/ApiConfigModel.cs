namespace TweetsAccessApi.Model
{
    public class ApiConfigModel
    {
        /// <summary>
        ///  Api key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        ///  Api key secret
        /// </summary>
        public string ApiKeySecret { get; set; }

        /// <summary>
        /// Api token
        /// </summary>
        public string BearerToken { get; set; }

        /// <summary>
        ///  Api endpoint url
        /// </summary>
        public string ApiUrl { get; set; }
    }
}

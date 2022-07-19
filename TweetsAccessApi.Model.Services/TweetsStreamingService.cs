using System;
using System.Threading.Tasks;
using TweetsAccessApi.Model.Services.Interfaces;
using TwitterAccessApi.Integration.Interfaces;

namespace TweetsAccessApi.Model.Services
{
    public class TweetsStreamingService : ITweetsStreamingService
    {
        private readonly ITwitterIntegration _twitterIntegration;

        public TweetsStreamingService(ITwitterIntegration twitterIntegration)
        {
            _twitterIntegration = twitterIntegration ?? throw new MissingMemberException($"{nameof(twitterIntegration)} is a required dependency;");
        }
        public async Task<TweetsRs> GetTweetsCount()
        {
            var tweets = await _twitterIntegration.GetTweetsSteamAsync();

            return new TweetsRs
            {
                TweetsReceivedCount = tweets.Data == null ? 0 :tweets.Data.Count
            };

        }
    }
}

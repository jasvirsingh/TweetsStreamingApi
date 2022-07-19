using System.Threading.Tasks;
using TwitterAccessApi.Integration.Models;

namespace TwitterAccessApi.Integration.Interfaces
{
    public interface ITwitterIntegration
    {
        /// <summary>
        /// Gets tweets stream
        /// </summary>
        Task<Tweets> GetTweetsSteamAsync();
    }
}

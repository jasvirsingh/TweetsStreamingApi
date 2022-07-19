using System.Threading.Tasks;

namespace TweetsAccessApi.Model.Services.Interfaces
{
    public interface ITweetsStreamingService
    {
        Task<TweetsRs> GetTweetsCount();
    }
}

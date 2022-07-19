using System.Security.Claims;
using System.Threading.Tasks;

namespace TweetsSteamingApi.Security
{
    public interface ITokenValidator
    {
        Task<bool> IsValidToken(string token);

        Task<ClaimsPrincipal> RetrieveClaimsPrincipalFromToken(string token);
    }
}

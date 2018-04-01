using System.Threading.Tasks;
using Domain.DataBaseModels.Identity;
using Domain.Identity;

namespace Servises.Interfaces.AuthenticationServices
{
    public interface IJwtTokensService
    {
        Task<JwtToken> GetJwtTokenByRefreshTokenAsync(string refreshToken);
        Task<JwtToken> GetJwtTokenAsync(ApplicationUser user);
        Task DeleteUserRefreshTokensAsync(ApplicationUser user);
    }
}
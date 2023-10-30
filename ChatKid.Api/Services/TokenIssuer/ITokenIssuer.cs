using ChatKid.Api.Services.TokenIssuer;
using System.Security.Claims;

namespace ChatKid.ApiFramework.AuthTokenIssuer
{
    public interface ITokenIssuer
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> GenerateJwtClaims(ClaimModel user, string[] roles);
        ClaimsPrincipal GetClaimsPrincipal(string token);
        Task<IEnumerable<Claim>> GenerateVerifyToken(string email, string role);
        Task<IEnumerable<Claim>> GenerateUserProfileClaims(string id, string role);
    }
}

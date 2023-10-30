using ChatKid.Api.Services.TokenIssuer;
using ChatKid.ApiFramework.Authentication;
using ChatKid.ApiFramework.AuthJwtIssuer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatKid.ApiFramework.AuthTokenIssuer
{
    public class TokenIssuer : ITokenIssuer
    {
        private readonly IJwtTokenIssuer jwtTokenIssuer;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly TrustedIssuerSettings trustedIssuerSettings;

        public TokenIssuer(
            IJwtTokenIssuer jwtTokenIssuer,
            AuthenticationSettings authenticationSettings,
            TrustedIssuerSettings trustedIssuerSettings)
        {
            this.jwtTokenIssuer = jwtTokenIssuer;
            this.authenticationSettings = authenticationSettings;
            this.trustedIssuerSettings = trustedIssuerSettings;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenId = Guid.Empty;
            return GenerateJwtToken(claims, DateTime.Now.AddMinutes(this.authenticationSettings.ExpiryMinutes), ref tokenId);
        }

        public async Task<IEnumerable<Claim>> GenerateJwtClaims(ClaimModel user, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "kidtalkie"),
                new Claim(CustomJwtRegisteredClaimNames.DisplayName, user.Email ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.Avatar, user.ImageUrl ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.ChatKidClaimName, "BeztTeamEver"),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<IEnumerable<Claim>> GenerateVerifyToken(string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "kidtalkie"),
                new Claim(CustomJwtRegisteredClaimNames.DisplayName, email ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.Scope, "/api/auth/otp-verify"),
            };

            claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public async Task<IEnumerable<Claim>> GenerateUserProfileClaims(string id, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "kidtalkie"),
                new Claim(CustomJwtRegisteredClaimNames.DisplayName, id),
            };

            claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiryTime, ref Guid tokenId)
        {
            return this.jwtTokenIssuer.GenerateJwtToken(claims, expiryTime.ToUniversalTime(), trustedIssuerSettings.Issuer, trustedIssuerSettings.Url, ref tokenId);
        }
    }
}

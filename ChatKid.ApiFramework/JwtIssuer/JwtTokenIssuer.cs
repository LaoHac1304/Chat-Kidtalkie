using ChatKid.ApiFramework.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatKid.ApiFramework.AuthJwtIssuer
{
    public class JwtTokenIssuer : IJwtTokenIssuer
    {
        private readonly AuthenticationSettings authenticationSettings;

        public JwtTokenIssuer(AuthenticationSettings authenticationSettings)
        {
            this.authenticationSettings = authenticationSettings;
        }

        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiryTime, string issuer, string audience, ref Guid tokenId)
        {
            tokenId = tokenId == Guid.Empty ? Guid.NewGuid() : tokenId;

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims.Concat(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, $"{tokenId}")
                }),
                DateTime.UtcNow,
                expiryTime,
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.authenticationSettings.Key)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            if (token == null)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authenticationSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return int.TryParse(jwtToken.Claims.First(x => x.Type == CustomJwtRegisteredClaimNames.UserId).Value, out int _);
            }
            catch
            {
                return false;
            }
        }
    }
}

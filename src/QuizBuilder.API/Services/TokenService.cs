using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizBuilder.Domain;
using QuizBuilder.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizBuilder.Services
{
    public class TokenService
    {
        private readonly TokenSettings _options;
        private SymmetricSecurityKey _key;
        public TokenService(IOptions<TokenSettings> options)
        {
            _options = options?.Value ??
                new TokenSettings { TokenValidityInMinutes = 60, RefreshTokenValidityInDays = 1, TokenKey = "DefaultKey" };
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.TokenKey));
        }

        public SecurityToken CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.TokenValidityInMinutes),
                SigningCredentials = credentials
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken)
                return null;

            return principal;
        }
    }
}

using System.Security.Cryptography;
using System.Text;
using laba2.Contracts.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace laba2.JWT;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtSettings _settings;

    public TokenProvider(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }
    
    public string GenerateAccessToken()
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SymmetricKey));
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMins),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}

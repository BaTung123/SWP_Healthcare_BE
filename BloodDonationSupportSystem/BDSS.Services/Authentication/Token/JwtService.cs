using BDSS.Common.Configurations;
using BDSS.Common.Utils;
using BDSS.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BDSS.Services.Authentication.Token;

public class JwtService : ITokenService
{
    #region Fields
    private readonly JwtConfigurations _config;
    #endregion

    #region Constructors
    public JwtService(JwtConfigurations config)
    {
        _config = config;
    }
    #endregion

    #region Public methods
    public string GetToken(User user)
    {
        return GetJwt(user);
    }
    #endregion

    #region Private methods
    private string GetJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTimeUtils.GetCurrentGmtPlus7().AddMinutes(_config.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            null,
            expiry,
            creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    #endregion
}

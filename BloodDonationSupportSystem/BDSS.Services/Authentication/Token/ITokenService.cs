using BDSS.Models.Entities;

namespace BDSS.Services.Authentication.Token;

public interface ITokenService
{
    public string GetToken(User user);
}

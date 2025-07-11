namespace BDSS.Services.Authentication.Hash;

public interface IPasswordHashingService
{
    public string GetHashedPassword(string password);

    public bool VerifyHashedPassword(string password, string hashedPassword);
}

using BDSS.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace BDSS.Services.Authentication.Hash;

public class PasswordHashingService : IPasswordHashingService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordHashingService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string GetHashedPassword(string password)
    {
        return _passwordHasher.HashPassword(new User(), password);
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(new User(), hashedPassword, password);
        Console.WriteLine(result);
        Console.WriteLine(hashedPassword);
        Console.WriteLine(password);
        return result == PasswordVerificationResult.Success;
    }
}

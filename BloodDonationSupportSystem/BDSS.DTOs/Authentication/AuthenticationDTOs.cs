using BDSS.Common.Enums;
using Microsoft.AspNetCore.Http;
namespace BDSS.DTOs.Authentication;

public class GetAllUsersResponse
{
    public IEnumerable<UsersInfo> Users { get; set; } = new List<UsersInfo>();
}

public class UsersInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public BDSS.Common.Enums.BloodType? BloodType { get; set; } = null;
}
public class GetUserByIdRequest
{
    public long Id { get; set; }
}

public class GetUserByIdResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Dob { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public BloodType BloodType { get; set; } = BloodType.O_Negative;
    public string AvatarImageUrl { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
}

public class UserLoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLoginResponse
{
    public string Token { get; set; } = string.Empty;
}

public class UserRegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class CreateDesignerAccountRequest
{
    public long UserId { get; set; }
    public float CommissionRate { get; set; }
}

public class CreateDesignerAccountResponse
{
    public bool Success { get; set; }
}

public class CreateManufacturerAccountRequest
{
    public long UserId { get; set; }
    public float CommissionRate { get; set; }
}

public class CreateManufacturerAccountResponse
{
    public bool Success { get; set; }
}

public class UpdateUserAvatarRequest
{
    public long Id { get; set; }
    public IFormFile Avatar { get; set; } = null!;
}

public class UpdateUserAvatarResponse
{
    public bool Success { get; set; }
}

public class UpdateUserStatusRequest
{
    public long Id { get; set; }
    public bool IsBanned { get; set; }
}

public class UpdateUserStatusResponse
{
    public bool Success { get; set; }
}

public class UserRegisterResponse
{
    public bool Success { get; set; }
}

public class UpdateUserPasswordRequest
{
    public long Id { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}

public class UpdateUserPasswordResponse
{
    public bool Success { get; set; }
}

public class UserDeleteRequest
{
    public long Id { get; set; }
    public string Password { get; set; } = null!;
}

public class UserDeleteResponse
{
    public bool Success { get; set; }
}

public class UpdateUserInformationRequest
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public string? Dob { get; set; }
    public string? PhoneNumber { get; set; }
    public BloodType? BloodType { get; set; }
}

public class UpdateUserInformationResponse
{
    public bool Success { get; set; }
}

// DTO mới để cập nhật UserRole
public class UpdateUserRoleRequest
{
    public long Id { get; set; }
    public float? CommissionRate { get; set; }
    public UserRole Role { get; set; }
}

public class UpdateUserRoleResponse
{
    public bool Success { get; set; }
}

public class SendEmailRequest
{
    public long UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsHtml { get; set; } = false;
}

public class SendEmailResponse
{
    public bool Success { get; set; }
}

public class ForgetUserPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ForgetUserPasswordResponse
{
    public bool Success { get; set; }
}
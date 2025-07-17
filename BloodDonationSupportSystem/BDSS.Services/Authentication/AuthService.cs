using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BDSS.Common.Enums;
using BDSS.Common.Utils;
using BDSS.DTOs;
using BDSS.DTOs.Authentication;
using BDSS.DTOs.UserOtp;
using BDSS.Models.Entities;
using BDSS.Repositories.UserOtpRepository;
using BDSS.Repositories.UserRepository;
using BDSS.Services.Authentication.Hash;
using BDSS.Services.Authentication.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
namespace BDSS.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUserOtpRepository _userOtpRepository;
    private readonly string? _azureConnectionString;
    private readonly string? _azureContainerName;

    public AuthService(
        IPasswordHashingService passwordHashingService,
        ITokenService tokenService,
        IUserRepository userRepository,
        IConfiguration configuration,
        IEmailService emailService,
        IUserOtpRepository userOtpRepository)
    {
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _configuration = configuration;
        _emailService = emailService;
        _userOtpRepository = userOtpRepository ?? throw new ArgumentNullException(nameof(userOtpRepository));
        _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
        _azureContainerName = _configuration["AzureStorage:ImagesContainer"];
    }

    #region Public Methods
    public string HashPassword(string password)
    {
        return _passwordHashingService.GetHashedPassword(password);
    }

    public async Task<BaseResponseModel<GetAllUsersResponse>> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAll()
                                            .Where(user => !user.IsDeleted)
                                            .ToListAsync();
            return new BaseResponseModel<GetAllUsersResponse>
            {
                Code = 200,
                Message = "Get all users successfully",
                Data = new GetAllUsersResponse
                {
                    Users = users.Select(user => new UsersInfo
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role.ToString(),
                        Status = user.IsBanned ? "Banned" : "Active",
                        CreatedAt = user.CreatedAt.ToString("dd-MM-yyyy"),
                        BloodType = user.BloodType
                    }).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllUsersResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<GetUserByIdResponse>> GetUserById(GetUserByIdRequest request)
    {
        try
        {
            var user = await _userRepository.FindAsync(request.Id);
            if (user == null || user.IsDeleted)
            {
                throw new InvalidOperationException("User not found");
            }
            return new BaseResponseModel<GetUserByIdResponse>
            {
                Code = 200,
                Message = "Get user by id successfully",
                Data = new GetUserByIdResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Dob = user.Dob?.ToString("dd-MM-yyyy") ?? string.Empty,
                    Gender = user.Gender ?? string.Empty,
                    BloodType = user.BloodType ?? BloodType.O_Negative,
                    AvatarImageUrl = user.AvatarUrl ?? string.Empty,
                    PhoneNumber = user.Phone ?? string.Empty,
                    IsVerified = user.IsVerified
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetUserByIdResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserLoginRequestAsync(request);
            if (user.IsBanned == true)
            {
                return new BaseResponseModel<UserLoginResponse>
                {
                    Code = 401,
                    Message = "This account has been banned"
                };
            }
            var token = _tokenService.GetToken(user);

            return new BaseResponseModel<UserLoginResponse>
            {
                Code = 200,
                Message = "Login successful",
                Data = new UserLoginResponse { Token = token }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserLoginResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserRegisterRequestAsync(request);
            await _userRepository.AddAsync(user);
            var otpRequest = new GenerateOtpRequest { Email = user.Email, PurposeType = "VerifyEmail", ExpiryTimeInMinutes = 5 };
            var otp = await GenerateOtpAsync(otpRequest);
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 200,
                Message = "User registered successfully",
                Data = new UserRegisterResponse { Success = true }
            };
        }
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 409, // Conflict for existing email
                Message = ex.Message,
                Data = new UserRegisterResponse { Success = false }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = ex.Message.Contains("not found") ? 404 : 400,
                Message = ex.Message
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserRegisterResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UserRegisterResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserStatusResponse>> UpdateUserStatus(UpdateUserStatusRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserStatusRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = 200,
                Message = "User status updated successfully",
                Data = new UpdateUserStatusResponse { Success = true }
            };
        }
        catch (InvalidOperationException ex)
        {
            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = ex.Message.Contains("not found") ? 404 : 400,
                Message = ex.Message,
                Data = new UpdateUserStatusResponse { Success = false }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserStatusResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserPasswordResponse>> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserPasswordRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 200,
                Message = "Password updated successfully",
                Data = new UpdateUserPasswordResponse { Success = true }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 400,
                Message = ex.Message,
                Data = new UpdateUserPasswordResponse { Success = false }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserPasswordResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<ForgetUserPasswordResponse>> ForgetUserPassword(ForgetUserPasswordRequest request)
    {
        try
        {
            var user = await GetUserEntityFromForgetUserPasswordRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<ForgetUserPasswordResponse>
            {
                Code = 200,
                Message = "Password updated successfully",
                Data = new ForgetUserPasswordResponse
                {
                    Success = true
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<ForgetUserPasswordResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserAvatarResponse>> UpdateUserAvatar(UpdateUserAvatarRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserAvatarRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = 200,
                Message = "User avatar updated successfully",
                Data = new UpdateUserAvatarResponse { Success = true }
            };
        }
        catch (InvalidOperationException iex)
        {
            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = iex.Message.Contains("not found") ? 404 : 400,
                Message = iex.Message
            };

        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserAvatarResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserInformationResponse>> UpdateUserInformation(UpdateUserInformationRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserInformationRequestAsync(request);
            await _userRepository.UpdateAsync(user);

            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 200,
                Message = "User information updated successfully",
                Data = new UpdateUserInformationResponse { Success = true }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 400,
                Message = ex.Message,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserInformationResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<UpdateUserRoleResponse>> UpdateUserRole(UpdateUserRoleRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUpdateUserRoleRequestAsync(request);

            await _userRepository.UpdateAsync(user);
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = 200,
                Message = "User role updated successfully",
                Data = new UpdateUserRoleResponse
                {
                    Success = true
                }
            };
        }
        catch (ArgumentException ex)
        {
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = ex.Message.Contains("not found") ? 404 : 400,
                Message = ex.Message,

            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UpdateUserRoleResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new UpdateUserRoleResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<UserDeleteResponse>> DeleteUser(UserDeleteRequest request)
    {
        try
        {
            var user = await GetUserEntityFromUserDeleteRequestAsync(request);
            await _userRepository.DeleteAsync(user.Id);

            return new BaseResponseModel<UserDeleteResponse>
            {
                Code = 200,
                Message = "User deleted successfully",
                Data = new UserDeleteResponse { Success = true }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<UserDeleteResponse>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponseModel<SendEmailResponse>> SendEmailToUser(SendEmailRequest request)
    {
        try
        {
            var user = await _userRepository.FindAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<SendEmailResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = new SendEmailResponse { Success = false }
                };
            }

            var emailSent = await _emailService.SendEmailAsync(
                user.Email,
                request.Subject,
                request.Body,
                request.IsHtml
            );

            return new BaseResponseModel<SendEmailResponse>
            {
                Code = emailSent ? 200 : 500,
                Message = emailSent ? "Email sent successfully" : "Failed to send email",
                Data = new SendEmailResponse { Success = emailSent }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<SendEmailResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new SendEmailResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<SendEmailResponse>> SendEmailToAdmin(SendEmailRequest request)
    {
        try
        {
            var user = await _userRepository.FindAsync(request.UserId);
            if (user == null)
            {
                return new BaseResponseModel<SendEmailResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = new SendEmailResponse { Success = false }
                };
            }
            var subject = $"From: {user.Email}. Subject: " + request.Subject;
            var emailSent = await _emailService.SendEmailAsync(
                "tunghoang5161997@gmail.com",
                subject,
                request.Body,
                request.IsHtml
            );

            return new BaseResponseModel<SendEmailResponse>
            {
                Code = emailSent ? 200 : 500,
                Message = emailSent ? "Email sent successfully" : "Failed to send email",
                Data = new SendEmailResponse { Success = emailSent }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<SendEmailResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new SendEmailResponse { Success = false }
            };
        }
    }

    public async Task<BaseResponseModel<GenerateOtpResponse>> GenerateOtpAsync(GenerateOtpRequest request)
    {
        try
        {
            // Check if user exists
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseResponseModel<GenerateOtpResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = null
                };
            }

            // Invalidate any existing OTPs for the same purpose
            var existingOtp = await _userOtpRepository.GetLatestOtpByUserIdAndPurposeAsync(user.Id, request.PurposeType);
            if (existingOtp != null)
            {
                existingOtp.IsUsed = true;
                await _userOtpRepository.UpdateAsync(existingOtp);
            }

            // Generate new OTP
            string otpCode = GenerateOtpCode();
            DateTime expiryTime = DateTimeUtils.GetCurrentGmtPlus7().AddMinutes(request.ExpiryTimeInMinutes);

            // Create new OTP record
            var newOtp = new UserOtp
            {
                UserId = user.Id,
                OtpCode = otpCode,
                ExpiryTime = expiryTime,
                IsUsed = false,
                PurposeType = request.PurposeType
            };

            await _userOtpRepository.AddAsync(newOtp);

            await SendEmailToUser(new SendEmailRequest
            {
                UserId = user.Id,
                Subject = $"OTP Verification for {request.PurposeType}",
                Body = $"Your OTP code is: {otpCode}",
                IsHtml = false
            });

            return new BaseResponseModel<GenerateOtpResponse>
            {
                Code = 200,
                Message = "OTP generated successfully",
                Data = new GenerateOtpResponse
                {
                    OtpCode = otpCode,
                    ExpiryTime = expiryTime
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GenerateOtpResponse>
            {
                Code = 500,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request)
    {
        try
        {
            // Check if user exists
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseResponseModel<VerifyOtpResponse>
                {
                    Code = 404,
                    Message = "User not found",
                    Data = new VerifyOtpResponse { IsValid = false }
                };
            }

            // Verify OTP
            bool isValid = await _userOtpRepository.VerifyOtpAsync(user.Id, request.OtpCode, request.PurposeType);

            if (isValid && request.PurposeType == "VerifyEmail")
            {
                user.IsVerified = true;
                await _userRepository.UpdateAsync(user);
            }

            return new BaseResponseModel<VerifyOtpResponse>
            {
                Code = isValid ? 200 : 400,
                Message = isValid ? "OTP verified successfully" : "Invalid or expired OTP",
                Data = new VerifyOtpResponse { IsValid = isValid }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<VerifyOtpResponse>
            {
                Code = 500,
                Message = $"An error occurred: {ex.Message}",
                Data = new VerifyOtpResponse { IsValid = false }
            };
        }
    }
    #endregion

    #region Private methods
    private bool IsValidEmail(string email)
    {
        try { var addr = new System.Net.Mail.MailAddress(email); return addr.Address == email; }
        catch { return false; }
    }
    private bool IsValidPassword(string password)
    {

        return password.Any(char.IsLetter) && password.Any(char.IsDigit);
    }
    private bool IsValidName(string name)
    {
        return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
    }
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 10 && phoneNumber.Length <= 12;
    }

    private async Task<User> GetUserEntityFromUserLoginRequestAsync(UserLoginRequest request)
    {
        if (request.Password.IsNullOrEmpty())
        {
            throw new InvalidOperationException("Password can not be empty");
        }
        if (request.Password.Length < 8)
        {
            throw new InvalidOperationException("Password can not less than 8 character ");
        }
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        return user;
    }

    private async Task<User> GetUserEntityFromUserRegisterRequestAsync(UserRegisterRequest request)
    {
        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty");

        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email can not be empty");
        if (!IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email format");

        // Validate password
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty");
        if (request.Password.Length < 8)
            throw new ArgumentException("Password can not less than 8 character");
        if (request.Password.Length > 20)
            throw new ArgumentException("Password can not greater than 20 character");
        if (!IsValidPassword(request.Password))
            throw new ArgumentException("Password does not format");

        // Validate confirm password
        if (request.ConfirmPassword != request.Password)
            throw new ArgumentException("Password not match");
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        return new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = _passwordHashingService.GetHashedPassword(request.Password),
            Role = UserRole.Customer,
            IsBanned = false,
            IsDeleted = false
        };
    }



    private async Task<User> GetUserEntityFromUpdateUserAvatarRequestAsync(UpdateUserAvatarRequest request)
    {
        if (request.Id <= 0)
        {
            throw new InvalidOperationException("Id can not be 0");
        }
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (request.Avatar == null)
        {
            throw new InvalidOperationException("Avatar is required");
        }
        // Validate file type (case-insensitive)
        var contentType = request.Avatar.ContentType?.ToLowerInvariant();
        if (contentType != "image/jpeg" && contentType != "image/png")
        {
            throw new InvalidOperationException("Only JPEG or PNG images are allowed");
        }

        DateTime gmtPlus7Time = DateTimeUtils.GetCurrentGmtPlus7();
        string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
        string fileName = $"avatar_{user.Id}_{formattedDateTime}.jpeg";
        byte[] fileBytes;

        using (var memoryStream = new MemoryStream())
        {
            await request.Avatar.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        var avatarPath = await UploadToAzureStorage(fileName, fileBytes);

        user.AvatarUrl = avatarPath;
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private async Task<string> UploadToAzureStorage(string fileName, byte[] fileBytes)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            // Determine content type by checking file signature (magic numbers)
            string contentType;
            if (fileBytes.Length >= 2)
            {
                // Check for PNG signature
                if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50)
                {
                    contentType = "image/png";
                }
                // Check for JPEG signature
                else if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
                {
                    contentType = "image/jpeg";
                }
                else
                {
                    throw new InvalidOperationException("Unsupported image format. Only PNG and JPEG are supported.");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid image file: File is too small or empty.");
            }

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = contentType
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
    }

    private async Task<User> GetUserEntityFromUpdateUserStatusRequestAsync(UpdateUserStatusRequest request)
    {
        if (request.Id <= 0)
        {
            throw new InvalidOperationException("Id must be greater than 0");
        }
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.Id} not found");
        }

        if (user.Role == UserRole.Admin)
        {
            throw new InvalidOperationException("Admin accounts cannot be banned");
        }




        user.IsBanned = request.IsBanned;
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private async Task<User> GetUserEntityFromForgetUserPasswordRequestAsync(ForgetUserPasswordRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        user.Password = _passwordHashingService.GetHashedPassword(request.Password);
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserPasswordRequestAsync(UpdateUserPasswordRequest request)
    {
        // Validate Id
        if (request.Id <= 0)
            throw new ArgumentException("Id required");

        // Validate current password
        if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            throw new ArgumentException("Current password is required");
        if (!IsValidPassword(request.CurrentPassword))
            throw new ArgumentException("Current password not format");
        if (request.CurrentPassword.Length < 8)
            throw new ArgumentException("Current password cannot be less than 8 character");
        if (request.CurrentPassword.Length > 20)
            throw new ArgumentException("Current password cannot be greater than 20 character");

        // Validate new password
        if (string.IsNullOrWhiteSpace(request.NewPassword))
            throw new ArgumentException("New password cannot be empty");
        if (!IsValidPassword(request.NewPassword))
            throw new ArgumentException("New password does not format");
        if (request.NewPassword.Length < 8)
            throw new ArgumentException("New password less than 8 character");
        if (request.NewPassword.Length > 20)
            throw new ArgumentException("New password greater than 20 character");

        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }
        if (!_passwordHashingService.VerifyHashedPassword(request.CurrentPassword, user.Password))
            throw new ArgumentException("Current password does not match");
        if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length < 8 || string.IsNullOrEmpty(request.CurrentPassword) || !_passwordHashingService.VerifyHashedPassword(request.CurrentPassword, user.Password))
        {
            throw new InvalidOperationException("Invalid password update request");
        }
        user.Password = _passwordHashingService.GetHashedPassword(request.NewPassword);
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserInformationRequestAsync(UpdateUserInformationRequest request)
    {
        // Validate Id
        if (request.Id <= 0)
            throw new ArgumentException("Id is required");

        // Validate Name
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is not in correct format (Name can not be empty)");
        if (!IsValidName(request.Name))
            throw new ArgumentException("Name is not in correct format (Name can not be empty)");
        if (request.Name.Length < 5)
            throw new ArgumentException("Name can not less than 5 characters");
        if (request.Name.Length > 25)
            throw new ArgumentException("Name can not greater than 25 characters");

        // Validate PhoneNumber
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new ArgumentException("PhoneNumber can not be empty");
        if (!IsValidPhoneNumber(request.PhoneNumber))
            throw new ArgumentException("Invalid phone format");

        // Validate Gender
        if (string.IsNullOrWhiteSpace(request.Gender))
            throw new ArgumentException("Gender can not be empty");
        if (!new[] { "male", "female", "other" }.Contains(request.Gender))
            throw new ArgumentException("Invalid gender value");

        // Validate Dob
        if (string.IsNullOrWhiteSpace(request.Dob))
            throw new ArgumentException("Date of birth can not be empty");
        if (!DateTime.TryParse(request.Dob, out _))
            throw new ArgumentException("Invalid date of birth");

        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }
        user.Name = request.Name ?? user.Name;
        user.Gender = request.Gender ?? user.Gender;
        user.Dob = !string.IsNullOrEmpty(request.Dob)
            ? DateTime.Parse(request.Dob)
            : user.Dob;
        user.Phone = request.PhoneNumber ?? user.Phone;
        user.BloodType = request.BloodType ?? user.BloodType;
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private async Task<User> GetUserEntityFromUserDeleteRequestAsync(UserDeleteRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null || !_passwordHashingService.VerifyHashedPassword(request.Password, user.Password))
        {
            throw new InvalidOperationException("Invalid user deletion request");
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private async Task<User> GetUserEntityFromUpdateUserRoleRequestAsync(UpdateUserRoleRequest request)
    {
        var user = await _userRepository.FindAsync(request.Id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }
        // Validate role
        //if (request.Role != UserRole.Customer)
        //{
        //    throw new ArgumentException("Role is invalid");
        //}
        // Validate CommissionRate
        //if (!request.CommissionRate.HasValue)
        //{
        //    throw new ArgumentException("Commission is reqiure");
        //}
        //if (request.CommissionRate.Value < 5)
        //{
        //    throw new ArgumentException("commissionRate can not less than 5");
        //}
        //if (request.CommissionRate.Value > 50)
        //{
        //    throw new ArgumentException("commissionRate can not greater than 50");
        //}

        user.Role = request.Role;
        user.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
        return user;
    }

    private string GenerateOtpCode()
    {
        // Generate a 6-digit OTP
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return (value % 1000000).ToString("D6"); // Ensure it's a 6-digit number
        }
    }
    #endregion
}
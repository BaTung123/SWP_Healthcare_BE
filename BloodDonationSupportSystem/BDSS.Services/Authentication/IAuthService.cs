using BDSS.DTOs;
using BDSS.DTOs.Authentication;
using BDSS.DTOs.UserOtp;

namespace BDSS.Services.Authentication;

public interface IAuthService
{
    string HashPassword(string password);
    Task<BaseResponseModel<GetAllUsersResponse>> GetAllUsers();
    Task<BaseResponseModel<GetUserByIdResponse>> GetUserById(GetUserByIdRequest request);
    Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request);
    Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request);
    Task<BaseResponseModel<UpdateUserStatusResponse>> UpdateUserStatus(UpdateUserStatusRequest request);
    Task<BaseResponseModel<ForgetUserPasswordResponse>> ForgetUserPassword(ForgetUserPasswordRequest request);
    Task<BaseResponseModel<UpdateUserPasswordResponse>> UpdateUserPassword(UpdateUserPasswordRequest request);
    Task<BaseResponseModel<UpdateUserInformationResponse>> UpdateUserInformation(UpdateUserInformationRequest request);
    Task<BaseResponseModel<UserDeleteResponse>> DeleteUser(UserDeleteRequest request);
    Task<BaseResponseModel<UpdateUserRoleResponse>> UpdateUserRole(UpdateUserRoleRequest request);
    Task<BaseResponseModel<UpdateUserAvatarResponse>> UpdateUserAvatar(UpdateUserAvatarRequest request);
    Task<BaseResponseModel<SendEmailResponse>> SendEmailToUser(SendEmailRequest request);
    Task<BaseResponseModel<SendEmailResponse>> SendEmailToAdmin(SendEmailRequest request);

    // OTP-related methods
    Task<BaseResponseModel<GenerateOtpResponse>> GenerateOtpAsync(GenerateOtpRequest request);
    Task<BaseResponseModel<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request);
}
using BDSS.DTOs.Authentication;
using BDSS.DTOs.UserOtp;
using BDSS.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("hashed-password/{password}")]
    public async Task<IActionResult> GetHashedPassword(string password)
    {
        var response = _authService.HashPassword(password);
        return StatusCode(200, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _authService.GetAllUsers();
        return StatusCode(response.Code, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] long id)
    {
        var request = new GetUserByIdRequest { Id = id };
        var response = await _authService.GetUserById(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var response = await _authService.Login(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _authService.Register(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SendEmailToUser([FromBody] SendEmailRequest request)
    {
        var response = await _authService.SendEmailToUser(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SendEmailToAdmin([FromBody] SendEmailRequest request)
    {
        var response = await _authService.SendEmailToAdmin(request);
        return StatusCode(response.Code, response);
    }

    [HttpGet("test")]
    public IActionResult TestLogin()
    {
        var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
        return Ok(token);
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordRequest request)
    {
        var response = await _authService.UpdateUserPassword(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("information")]
    public async Task<IActionResult> UpdateUserInformation([FromBody] UpdateUserInformationRequest request)
    {
        var response = await _authService.UpdateUserInformation(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("avatar")]
    public async Task<IActionResult> UpdateUserAvatar([FromForm] UpdateUserAvatarRequest request)
    {
        var response = await _authService.UpdateUserAvatar(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetUserPassword([FromBody] ForgetUserPasswordRequest request)
    {
        var response = await _authService.ForgetUserPassword(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
    {
        var response = await _authService.UpdateUserRole(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusRequest request)
    {
        var response = await _authService.UpdateUserStatus(request);
        return StatusCode(response.Code, response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        var response = await _authService.DeleteUser(request);
        return StatusCode(response.Code, response);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
    {
        var response = await _authService.GenerateOtpAsync(request);
        return StatusCode(response.Code, response);
    }

    [HttpPut("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var response = await _authService.VerifyOtpAsync(request);
        return StatusCode(response.Code, response);
    }
}
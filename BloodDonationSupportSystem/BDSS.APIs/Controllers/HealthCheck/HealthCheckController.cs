using BDSS.DTOs.HealthCheck;
using BDSS.Services.HealthCheck;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.HealthCheck;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HealthCheckController : ControllerBase
{
    private readonly IHealthCheckService _healthCheckService;

    public HealthCheckController(IHealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllHealthChecks()
    {
        var result = await _healthCheckService.GetAllHealthChecksAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHealthCheckById(long id)
    {
        var request = new GetHealthCheckByIdRequest { Id = id };
        var result = await _healthCheckService.GetHealthCheckByIdAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetHealthChecksByUserId(long userId)
    {
        var request = new GetHealthChecksByUserIdRequest { UserId = userId };
        var result = await _healthCheckService.GetHealthChecksByUserIdAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet("blood-donation-application/{bloodDonationApplicationId}")]
    public async Task<IActionResult> GetHealthChecksByBloodDonationApplicationId(long bloodDonationApplicationId)
    {
        var request = new GetHealthChecksByBloodDonationApplicationIdRequest { BloodDonationApplicationId = bloodDonationApplicationId };
        var result = await _healthCheckService.GetHealthChecksByBloodDonationApplicationIdAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHealthCheck([FromBody] CreateHealthCheckRequest request)
    {
        var result = await _healthCheckService.CreateHealthCheckAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateHealthCheck([FromBody] UpdateHealthCheckRequest request)
    {
        var result = await _healthCheckService.UpdateHealthCheckAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHealthCheck(long id)
    {
        var result = await _healthCheckService.DeleteHealthCheckAsync(id);
        return StatusCode(result.Code, result);
    }
} 
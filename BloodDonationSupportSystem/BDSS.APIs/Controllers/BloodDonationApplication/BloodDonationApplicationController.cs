using BDSS.DTOs.BloodDonationApplication;
using BDSS.Services.BloodDonationApplication;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.BloodDonationApplication;

[ApiController]
[Route("api/[controller]")]
public class BloodDonationApplicationController : ControllerBase
{
    private readonly IBloodDonationApplicationService _service;
    public BloodDonationApplicationController(IBloodDonationApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBloodDonationApplicationsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetBloodDonationApplicationByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(long userId)
    {
        var result = await _service.GetBloodDonationApplicationByUserIdAsync(userId);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBloodDonationApplicationRequest request)
    {
        var result = await _service.CreateBloodDonationApplicationAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBloodDonationApplicationRequest request)
    {
        var result = await _service.UpdateBloodDonationApplicationAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateBloodDonationApplicationStatusRequest request)
    {
        var result = await _service.UpdateBloodDonationApplicationStatusAsync(request);
        return StatusCode(result.Code, result);
    }
}
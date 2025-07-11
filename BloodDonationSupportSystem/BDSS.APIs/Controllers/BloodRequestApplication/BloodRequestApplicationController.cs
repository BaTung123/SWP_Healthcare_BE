using BDSS.DTOs.BloodRequestApplication;
using BDSS.Services.BloodRequestApplication;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.BloodRequestApplication;

[ApiController]
[Route("api/[controller]")]
public class BloodRequestApplicationController : ControllerBase
{
    private readonly IBloodRequestApplicationService _service;
    public BloodRequestApplicationController(IBloodRequestApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBloodRequestApplicationsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetBloodRequestApplicationByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBloodRequestApplicationRequest request)
    {
        var result = await _service.CreateBloodRequestApplicationAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateBloodRequestApplicationStatusRequest request)
    {
        var result = await _service.UpdateBloodRequestApplicationStatusAsync(request);
        return StatusCode(result.Code, result);
    }
}
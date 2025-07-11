using BDSS.DTOs.BloodExport;
using BDSS.Services.BloodExport;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.BloodExport;

[ApiController]
[Route("api/[controller]")]
public class BloodExportController : ControllerBase
{
    private readonly IBloodExportService _bloodExportService;
    public BloodExportController(IBloodExportService bloodExportService)
    {
        _bloodExportService = bloodExportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bloodExportService.GetAllBloodExportsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _bloodExportService.GetBloodExportByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBloodExportRequest request)
    {
        var result = await _bloodExportService.CreateBloodExportAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateBloodExportStatusRequest request)
    {
        var result = await _bloodExportService.UpdateBloodExportStatusAsync(request);
        return StatusCode(result.Code, result);
    }
}
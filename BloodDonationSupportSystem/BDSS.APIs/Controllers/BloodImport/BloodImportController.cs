using BDSS.DTOs.BloodImport;
using BDSS.Services.BloodImport;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.BloodImport;

[ApiController]
[Route("api/[controller]")]
public class BloodImportController : ControllerBase
{
    private readonly IBloodImportService _bloodImportService;
    public BloodImportController(IBloodImportService bloodImportService)
    {
        _bloodImportService = bloodImportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bloodImportService.GetAllBloodImportsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _bloodImportService.GetBloodImportByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBloodImportRequest request)
    {
        var result = await _bloodImportService.CreateBloodImportAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateBloodImportStatusRequest request)
    {
        var result = await _bloodImportService.UpdateBloodImportStatusAsync(request);
        return StatusCode(result.Code, result);
    }
}
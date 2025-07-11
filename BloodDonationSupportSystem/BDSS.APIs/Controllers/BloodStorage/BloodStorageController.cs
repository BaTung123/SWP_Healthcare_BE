using BDSS.DTOs.BloodStorage;
using BDSS.Services.BloodStorage;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.BloodStorage;

[ApiController]
[Route("api/[controller]")]
public class BloodStorageController : ControllerBase
{
    private readonly IBloodStorageService _bloodStorageService;
    public BloodStorageController(IBloodStorageService bloodStorageService)
    {
        _bloodStorageService = bloodStorageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bloodStorageService.GetAllBloodStoragesAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _bloodStorageService.GetBloodStorageByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBloodStorageRequest request)
    {
        var result = await _bloodStorageService.CreateBloodStorageAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBloodStorageRequest request)
    {
        var result = await _bloodStorageService.UpdateBloodStorageAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _bloodStorageService.DeleteBloodStorageAsync(id);
        return StatusCode(result.Code, result);
    }
}
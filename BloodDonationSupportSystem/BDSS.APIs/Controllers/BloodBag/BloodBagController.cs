using BDSS.DTOs.BloodBag;
using BDSS.Services.BloodBag;
using Microsoft.AspNetCore.Mvc;
using BDSS.Common.Enums;

namespace BDSS.APIs.Controllers.BloodBag;

[ApiController]
[Route("api/[controller]")]
public class BloodBagController : ControllerBase
{
    private readonly IBloodBagService _bloodBagService;

    public BloodBagController(IBloodBagService bloodBagService)
    {
        _bloodBagService = bloodBagService;
    }

    [HttpPost]
    public async Task<ActionResult<BloodBagDto>> CreateBloodBag([FromBody] CreateBloodBagRequest request)
    {
        try
        {
            var bloodBag = await _bloodBagService.CreateBloodBagAsync(request);
            return CreatedAtAction(nameof(GetBloodBagById), new { id = bloodBag.Id }, bloodBag);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BloodBagDto>> GetBloodBagById(long id)
    {
        try
        {
            var bloodBag = await _bloodBagService.GetBloodBagByIdAsync(id);
            return Ok(bloodBag);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetAllBloodBagsResponse>> GetAllBloodBags()
    {
        try
        {
            var bloodBags = await _bloodBagService.GetAllBloodBagsAsync();
            return Ok(new GetAllBloodBagsResponse { BloodBags = bloodBags });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<ActionResult<BloodBagDto>> UpdateBloodBag([FromBody] UpdateBloodBagRequest request)
    {
        try
        {
            var bloodBag = await _bloodBagService.UpdateBloodBagAsync(request);
            return Ok(bloodBag);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBloodBag(long id)
    {
        try
        {
            var result = await _bloodBagService.DeleteBloodBagAsync(id);
            if (!result)
                return NotFound(new { message = "Blood bag not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<BloodBagDto>>> GetAvailableBloodBags()
    {
        try
        {
            var bloodBags = await _bloodBagService.GetAvailableBloodBagsAsync();
            return Ok(bloodBags);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("blood-type/{bloodType}")]
    public async Task<ActionResult<IEnumerable<BloodBagDto>>> GetBloodBagsByBloodType(BloodType bloodType)
    {
        try
        {
            var bloodBags = await _bloodBagService.GetBloodBagsByBloodTypeAsync(bloodType);
            return Ok(bloodBags);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("expired")]
    public async Task<ActionResult<IEnumerable<BloodBagDto>>> GetExpiredBloodBags()
    {
        try
        {
            var bloodBags = await _bloodBagService.GetExpiredBloodBagsAsync();
            return Ok(bloodBags);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 
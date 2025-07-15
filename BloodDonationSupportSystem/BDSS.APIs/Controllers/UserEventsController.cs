using BDSS.DTOs.UserEvents;
using BDSS.Services.UserEvents;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BDSS.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserEventsController : ControllerBase
{
    private readonly IUserEventsService _userEventsService;
    public UserEventsController(IUserEventsService userEventsService)
    {
        _userEventsService = userEventsService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserToEvent([FromBody] RegisterUserToEventRequest request)
    {
        var result = await _userEventsService.RegisterUserToEventAsync(request);
        return StatusCode(result.Code, result);
    }
}
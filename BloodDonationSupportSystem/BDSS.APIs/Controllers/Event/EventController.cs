using BDSS.DTOs.Event;
using BDSS.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers.Event;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eventService.GetAllEventsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var result = await _eventService.CreateEventAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEventRequest request)
    {
        var result = await _eventService.UpdateEventAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        return StatusCode(result.Code, result);
    }
}
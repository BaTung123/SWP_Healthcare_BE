using BDSS.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BDSS.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BackgroundServiceController : ControllerBase
{
    [HttpGet("status")]
    public IActionResult GetServiceStatus()
    {
        return Ok(new
        {
            message = "Background service is running",
            service = "ExpiredBloodBagService",
            status = "Active",
            timestamp = DateTimeUtils.GetCurrentGmtPlus7()
        });
    }
}
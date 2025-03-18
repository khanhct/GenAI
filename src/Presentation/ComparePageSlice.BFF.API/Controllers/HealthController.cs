using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComparePageSlice.BFF.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Health check endpoint called 111111111111111111");
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
} 
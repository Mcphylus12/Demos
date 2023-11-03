using System.ComponentModel.DataAnnotations;
using Communication.Abstractions;
using DemoCore;
using Microsoft.AspNetCore.Mvc;

namespace DemoEntry.Controllers;
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{

    private readonly ILogger<DemoController> _logger;
    private readonly ISender _sender;

    public DemoController(ILogger<DemoController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    [Route("SendRequest")]
    public async Task<IActionResult> SendRequest([FromQuery][Required] string value)
    {
        _logger.LogInformation("Sending request value={value}", value);
        await _sender.Send(new DemoRequest
        {
            Data = value
        });
        _logger.LogInformation("Sent request value={value}", value);
        return Ok();
    }

    [HttpPost]
    [Route("SendMessage")]
    public async Task<IActionResult> SendMessage([FromQuery][Required] string value)
    {
        _logger.LogInformation("Sending message value={value}", value);
        await _sender.Send(new DemoMessage
        {
            Data = value
        });
        _logger.LogInformation("Sent message value={value}", value);
        return Ok();
    }
}

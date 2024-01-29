using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WalletService.Presentation.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class TelegramController(ILoggerManager logger) : ControllerBase
{
    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        logger.LogInfo("Test inbound connection from TelegramService");
        return Ok("Test inbound connection from TelegramService");
    }
}
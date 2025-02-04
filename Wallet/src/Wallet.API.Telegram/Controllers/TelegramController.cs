using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Domain.Contracts;

namespace Wallet.API.Telegram.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramController(ILoggerManager logger) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult TestInboundConnection()
    {
        logger.LogInfo("Test inbound connection from Wallet.Services.Telegram");
        return Ok("Test inbound connection from Wallet.Services.Telegram");
    }
}

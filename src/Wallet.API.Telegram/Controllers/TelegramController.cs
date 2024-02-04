using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Domain.Contracts;

namespace Wallet.API.Telegram.Controllers;

[Authorize]
[Route("api/c/[controller]")]
[ApiController]
public class TelegramController(ILoggerManager logger) : ControllerBase
{
    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        logger.LogInfo("Test inbound connection from Wallet.Services.Telegram");
        return Ok("Test inbound connection from Wallet.Services.Telegram");
    }
}
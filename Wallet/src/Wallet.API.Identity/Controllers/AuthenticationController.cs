using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Identity.Commands.Authetication;
using Wallet.Shared.DataTransferObjects;
using Wallet.Shared.Extensions;

namespace Wallet.API.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto user)
    {
        var result = await sender.Send(new LoginCommand(user.EnsureExists()));
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var token = await sender.Send(new GenerateTokenCommand(user, false));
        return Ok(token);
    }

    [Authorize]
    [HttpPost("test")]
    public IActionResult TestInboundConnection()
    {
        return Ok("Test inbound connection from Wallet.API.Identity");
    }
}
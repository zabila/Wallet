using Application.Authetication.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Identity.Controllers;

public sealed record Request(string Email, string Password);

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Request request)
    {
        var result = await sender.Send(new LoginCommand(request.Email, request.Password));
        return Ok(result);
    }

    [Authorize]
    [HttpPost("test")]
    public IActionResult TestInboundConnection()
    {
        return Ok("Test inbound connection from Wallet.API.Identity");
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Identity.Commands.Authetication;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Identity.Controllers;

public class AuthenticationController(ISender sender) : ControllerBase {

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForAuthenticationDto user) {
        var result = await sender.Send(new LoginCommand(user));
        if (!result.Succeeded) return Unauthorized();

        var token = await sender.Send(new GenerateTokenCommand(user, false));
        return Ok(token);
    }
}

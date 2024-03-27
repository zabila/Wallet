using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Identity.Commands.User;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Identity.Controllers;

[Route("api/identity")]
[ApiController]
public class UserController(ISender sender) : ControllerBase {
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> Register(RegisterUserDto model) {
        var result = await sender.Send(new RegisterUserCommand(model));
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok();
    }

    [Authorize]
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserDto model) {
        var result = await sender.Send(new UpdateUserCommand(model));
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole(AssignRoleDto model) {
        await sender.Send(new AssignRoleCommand(model));
        return Ok();
    }
}
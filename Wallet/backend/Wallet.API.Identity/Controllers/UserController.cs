using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Identity.Commands.User;
using Wallet.Application.Identity.Queries;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase {
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto model) {
        var result = await sender.Send(new RegisterUserCommand(model));
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        return Ok();
    }

    [Authorize]
    [HttpGet("GetCurrent")]
    public async Task<IActionResult> GetCurrentUser(string username) {
        var result = await sender.Send(new GetCurrentUserQuery(username));
        return Ok(result);
    }

    [Authorize]
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model) {
        var result = await sender.Send(new UpdateUserCommand(model));
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        return Ok();
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model) {
        await sender.Send(new AssignRoleCommand(model));
        return Ok();
    }
}

using Application.Users.Register;
using Application.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedKernel.DTO.Users;

namespace Wallet.API.Identity.Controllers;

[Authorize]
[Route("api/user")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest model, CancellationToken cancellationToken)
    {

        var command = new RegisterUserCommand
        {
            Email = model.Email,
            Password = model.Password,
            FirstName = model.FirstName,
            LastName = model.LastName,
            TelegramUserId = model.TelegramUserId,
            TelegramUsername = model.TelegramUsername,
            Localization = model.Localization
        };

        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{userId: Guid}/Update")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest model, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand
        {
            UserId = userId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            TelegramUserId = model.TelegramUserId,
            TelegramUsername = model.TelegramUsername,
            Localization = model.Localization
        };

        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }
}

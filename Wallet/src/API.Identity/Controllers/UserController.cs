using API.Identity.Extensions;
using API.Identity.Infrastructure;
using Application.Users.GetUserByTelegramId;
using Application.Users.Register;
using Application.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.DTO.Login;
using SharedKernel.DTO.Users;

namespace Wallet.API.Identity.Controllers;

[Authorize]
[Route("api/user")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
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

        Result<TokenResponse> result = await sender.Send(command, cancellationToken);
        return result.Match(Ok, error => CustomResults.Problem(error));
    }

    [HttpPatch("{userId:guid}/update")]
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

        Result result = await sender.Send(command, cancellationToken);
        return result.Match(Ok, error => CustomResults.Problem(error));
    }

    [HttpGet("telegram/{telegrmId:long}")]
    public async Task<IActionResult> GetUserByTelegramId([FromRoute] long telegrmId, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await sender.Send(new GetUserByTelegramIdQuery(telegrmId), cancellationToken);
        return result.Match(Ok, error => CustomResults.Problem(error));
    }
}

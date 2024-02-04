using Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace WalletService.Presentation.Controllers;

[Route("api/identity")]
[ApiController]
public class UserController(UserManager<User> userManager) : ControllerBase
{
    [HttpPost("registerUser")]
    public async Task<IActionResult> Register(RegisterUserDto model)
    {
        var user = new User()
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            TelegramUserId = model.TelegramUserId,
            TelegramUsername = model.TelegramUsername
        };
        var result = await userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser(RegisterUserDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email!);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.TelegramUserId = model.TelegramUserId;
        user.TelegramUsername = model.TelegramUsername;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}
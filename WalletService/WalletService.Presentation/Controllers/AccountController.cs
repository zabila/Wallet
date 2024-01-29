using Application.Account.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace WalletService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto accountForCreationDto)
    {
        var account = await sender.Send(new CreateAccountCommand(accountForCreationDto));
        return Ok(account);
    }
}
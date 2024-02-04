using Application.Account.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace WalletService.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto accountForCreationDto)
    {
        var account = await sender.Send(new CreateAccountCommand(accountForCreationDto));
        return Ok(account);
    }
}
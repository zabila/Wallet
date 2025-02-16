using Application.Accounts.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.DTO.Accounts;

namespace API.Finance.Controllers;

[Authorize]
[Route("api/user/{userid:guid}/account")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromRoute] Guid userid, [FromBody] AccountCreateRequest accountCreateRequest, CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand
        {
            UserId = userid,
            AccountName = accountCreateRequest.AccountName,
            AccountType = accountCreateRequest.AccountType,
            Currency = accountCreateRequest.Currency
        };

        var account = await sender.Send(command, cancellationToken);
        return Ok(account);
    }
}

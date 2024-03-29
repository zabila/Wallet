﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Finance.Account.Commands;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Finance.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase {
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto accountForCreationDto) {
        var account = await sender.Send(new CreateAccountCommand(accountForCreationDto));
        return Ok(account);
    }
}
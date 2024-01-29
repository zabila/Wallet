using Application.Transaction.Queries;
using Application.Transaction.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Shared.DataTransferObjects;

namespace WalletService.Presentation.Controllers;

[Route("api/{accountId:guid}/[controller]")]
[ApiController]
public class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTransaction(Guid accountId, [FromBody] TransactionCreateDto transactionForCreationDto)
    {
        var transaction = await sender.Send(new CreateTransactionCommand(accountId, transactionForCreationDto));
        return Ok(transaction);
    }

    [HttpGet("{transactionId:guid}", Name = "TransactionById")]
    [Authorize]
    public async Task<IActionResult> GetTransaction(Guid accountId, Guid transactionId)
    {
        var transaction = await sender.Send(new GetTransactionQuery(accountId, transactionId, TrackChanges: false));
        return Ok(transaction);
    }
}
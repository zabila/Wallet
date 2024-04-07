using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Finance.Transaction.Commands;
using Wallet.Application.Finance.Transaction.Queries;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.API.Finance.Controllers;

[Route("api/account/{accountId:guid}/[controller]")]
[ApiController]
public class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction(
        Guid accountId,
        [FromBody] TransactionCreateDto transactionForCreationDto
    )
    {
        var transaction = await sender.Send(
            new CreateTransactionCommand(accountId, transactionForCreationDto)
        );
        return CreatedAtRoute(
            "TransactionById",
            new { accountId, transactionId = transaction.Id },
            transaction
        );
    }

    [HttpGet("{transactionId:guid}", Name = "TransactionById")]
    public async Task<IActionResult> GetTransaction(Guid accountId, Guid transactionId)
    {
        var transaction = await sender.Send(
            new GetTransactionQuery(accountId, transactionId, TrackChanges: false)
        );
        return Ok(transaction);
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(Guid accountId)
    {
        var transactions = await sender.Send(
            new GetTransactionsQuery(accountId, TrackChanges: false)
        );
        return Ok(transactions);
    }
}

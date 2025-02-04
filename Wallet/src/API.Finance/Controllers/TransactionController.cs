using Application.Transactions.Create;
using Application.Transactions.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.DTO.Transactions;

namespace API.Finance.Controllers;

[Route("api/account/{userid:guid}/transactions")]
[ApiController]
public class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction([FromRoute] Guid userid, [FromBody] TransactionsRequest transactionsRequest, CancellationToken cancellationToken)
    {
        var createTransactionCommand = new CreateTransactionCommand
        {
            UserId = userid,
            Amount = transactionsRequest.Amount,
            Category = transactionsRequest.Category,
            Type = transactionsRequest.Type,
            Location = transactionsRequest.Location,
            Attachment = transactionsRequest.Attachment
        };

        var transaction = await sender.Send(createTransactionCommand, cancellationToken);
        return Ok(transaction);
    }

    [HttpGet()]
    public async Task<IActionResult> GetTransactions([FromRoute] Guid userid, CancellationToken cancellationToken)
    {
        var transactions = await sender.Send(new GetTransactionsQuery(userid), cancellationToken);
        return Ok(transactions);
    }
}

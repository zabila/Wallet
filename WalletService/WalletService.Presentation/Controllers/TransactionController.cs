using Application.Commands;
using Application.Queries.Transaction;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Shared.DataTransferObjects;

namespace WalletService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionForCreationDto transactionForCreationDto)
    {
        var transaction = await sender.Send(new CreateTransactionCommand(transactionForCreationDto));

        return CreatedAtRoute("TransactionById", new { id = transaction.Id }, transaction);
    }

    [HttpGet("{id:guid}", Name = "TransactionById")]
    [Authorize]
    public async Task<IActionResult> GetTransaction(Guid id)
    {
        var transaction = await sender.Send(new GetTransactionQuery(id, TrackChanges: false));
        return Ok(transaction);
    }
}
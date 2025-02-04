using Wallet.Services.Telegram.Dtos;

namespace Wallet.Services.Telegram.AsyncDataServices;

public interface IMessageBusClient
{
    Task PublishNewTransactionAsync(TransactionPublishedDto transactionPublishedDto);
}
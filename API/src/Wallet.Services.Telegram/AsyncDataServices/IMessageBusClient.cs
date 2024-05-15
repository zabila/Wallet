using Wallet.Services.Telegram.Dtos;

namespace Wallet.Services.Telegram.AsyncDataServices;

public interface IMessageBusClient {
    void PublishNewTransaction(TransactionPublishedDto transactionPublishedDto);
}
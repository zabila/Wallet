using TelegramService.Dtos;

namespace TelegramService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewTransaction(TransactionPublishedDto transactionPublishedDto);
}
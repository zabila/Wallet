namespace TelegramService.SyncDataServices.Http;

public interface IWalletDataClient
{
    Task TestInboundConnection();
}
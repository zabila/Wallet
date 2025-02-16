namespace API.Telegram.Contracts;

public interface IWalletFinanceAccountClient
{
    Task TestInboundConnectionAsync();
    Task<List<string>> GetIncomingCategoriesAsync();
    Task<List<string>> GetOutcomingCategoriesAsync();
}

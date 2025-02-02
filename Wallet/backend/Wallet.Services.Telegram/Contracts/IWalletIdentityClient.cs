namespace Wallet.Services.Telegram.Contracts;

public interface IWalletIdentityClient {
    Task TestInboundConnectionAsync();
}
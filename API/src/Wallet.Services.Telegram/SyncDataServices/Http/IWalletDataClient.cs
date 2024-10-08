﻿namespace Wallet.Services.Telegram.SyncDataServices.Http;

public interface IWalletDataClient {
    Task TestInboundConnection();
    Task<List<string>> GetIncomingCategoriesAsync();
    Task<List<string>> GetOutcomingCategoriesAsync();
}
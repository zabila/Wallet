using Wallet.Domain.Entities.Model;

namespace Wallet.Domain.Contracts;

public interface IWalletIdentityUsersRepository {
    Task<WalletIdentityUser?> FindUserByTelegramUserIdAsync(int telegramUserId, bool trackChanges, CancellationToken cancellationToken);
}
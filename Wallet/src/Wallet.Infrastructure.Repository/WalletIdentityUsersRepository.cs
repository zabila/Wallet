using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;

namespace Wallet.Infrastructure.Repository;

public class WalletIdentityUsersRepository(DbContext repositoryContext) : RepositoryBase<WalletIdentityUser>(repositoryContext), IWalletIdentityUsersRepository
{
    public Task<WalletIdentityUser?> FindUserByTelegramUserIdAsync(int telegramUserId, bool trackChanges, CancellationToken cancellationToken)
    {
        return FindByCondition(u => u.TelegramUserId.Equals(telegramUserId), trackChanges).SingleOrDefaultAsync(cancellationToken);
    }
}
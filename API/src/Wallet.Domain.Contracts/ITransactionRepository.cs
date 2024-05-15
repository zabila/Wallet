using Wallet.Domain.Entities.Model;

namespace Wallet.Domain.Contracts;

public interface ITransactionRepository {
    void CreateTransaction(Transaction transaction);
    Task<Transaction> GetTransactionAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync(bool trackChanges, CancellationToken cancellationToken);
}
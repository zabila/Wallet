using Contracts;
using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public sealed class TransactionRepository(DbContext repositoryContext) : RepositoryBase<Transaction>(repositoryContext), ITransactionRepository
{
    public void CreateTransaction(Transaction transaction)
    {
        Create(transaction);
    }

    public async Task<Transaction> GetTransactionAsync(Guid id, bool trackChanges, CancellationToken cancellationToken)
    {
        return (await FindByCondition(t => t.Id.Equals(id), trackChanges).SingleOrDefaultAsync(cancellationToken))!;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindAll(trackChanges).ToListAsync(cancellationToken);
    }
}
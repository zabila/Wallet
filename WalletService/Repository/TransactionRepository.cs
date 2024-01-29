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

    public Transaction? GetTransaction(Guid id, bool trackChanges)
    {
        return FindByCondition(t => t.Id.Equals(id), trackChanges).SingleOrDefault();
    }

    public IQueryable<string?> GetAllCategories(bool trackChanges) => FindAll(trackChanges).Select(t => t.Category).Distinct();
}
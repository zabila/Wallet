using Entities.Model;

namespace Contracts;

public interface ITransactionRepository
{
    void CreateTransaction(Transaction transaction);

    Transaction? GetTransaction(Guid id, bool trackChanges);

    IQueryable<string?> GetAllCategories(bool trackChanges);
}
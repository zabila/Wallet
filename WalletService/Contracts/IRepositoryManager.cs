namespace Contracts;

public interface IRepositoryManager
{
    ITransactionRepository Transaction { get; }

    Task SaveAsync();
}
namespace Infrastructure.Abstractions;
public interface IRepositoryContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

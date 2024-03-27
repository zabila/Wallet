using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wallet.Domain.Contracts;

namespace Wallet.Infrastructure.Repository;

public class RepositoryBase<T>(DbContext repositoryContext) : IRepositoryBase<T> where T : class
{
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return trackChanges ? repositoryContext.Set<T>() : repositoryContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    {
        return trackChanges
            ? repositoryContext.Set<T>().Where(expression)
            : repositoryContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    {
        SetTimestamps(entity);
        repositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        SetTimestamps(entity);
        repositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        repositoryContext.Set<T>().Remove(entity);
    }

    private void SetTimestamps(T entity)
    {
        var entry = repositoryContext.Entry(entity);

        if (entry.Metadata.FindProperty("CreatedAt") != null)
            entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;

        if (entry.Metadata.FindProperty("UpdatedAt") != null)
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
    }
}
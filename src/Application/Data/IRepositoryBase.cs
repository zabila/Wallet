using System.Linq.Expressions;

namespace Application.Data;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    Task CreateAsync(T entity, CancellationToken cancellation);
    void Update(T entity);
    void Delete(T entity);
}

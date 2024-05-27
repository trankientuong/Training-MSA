using System.Linq.Expressions;

namespace MSA.Common.Contracts.Domain;

public interface IRepository<T> where T : IEntity
{
    Task CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(Guid id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task UpdateAsync(T entity);
}
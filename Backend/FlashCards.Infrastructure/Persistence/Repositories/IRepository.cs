namespace FlashCards.Infrastructure.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<bool> Exists(Guid id);
}
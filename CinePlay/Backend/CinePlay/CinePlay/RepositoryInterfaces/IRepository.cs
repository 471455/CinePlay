namespace MyMDb.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task SaveChangesAsync();
    }
}

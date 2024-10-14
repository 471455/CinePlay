using MyMDb.Models;

namespace MyMDb.RepositoryInterfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        public Task<Review?> GetByUserAsync(string userId, Guid mediaId);
    }
}

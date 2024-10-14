using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyMDb.Data;
using MyMDb.Models;
using MyMDb.RepositoryInterfaces;

namespace MyMDb.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Review?> GetByUserAsync(string userId, Guid mediaId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MediaId == mediaId);
        }
    }
}

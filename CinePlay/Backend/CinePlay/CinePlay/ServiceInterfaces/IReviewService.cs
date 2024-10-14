using MyMDb.DTOs;
using MyMDb.Models;

namespace MyMDb.ServiceInterfaces
{
    public interface IReviewService
    {
        public Task<Review> AddReview(string userId, ReviewDto reviewDto);
        public Task<Review?> GetByUserAsync(string userId, Guid mediaId);
        public Task DeleteReview(Review review);
    }
}

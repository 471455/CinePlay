using AutoMapper;
using MyMDb.DTOs;
using MyMDb.Models;
using MyMDb.RepositoryInterfaces;
using MyMDb.ServiceInterfaces;

namespace MyMDb.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IMapper mapper, IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> AddReview(string userId, ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.UserId = userId;
            review.Initialize();
            return await _reviewRepository.AddAsync(review);
        }

        public async Task DeleteReview(Review review)
        {
            await _reviewRepository.Delete(review);
        }

        public async Task<Review?> GetByUserAsync(string userId, Guid mediaId)
        {
            return await _reviewRepository.GetByUserAsync(userId, mediaId);
        }
    }
}

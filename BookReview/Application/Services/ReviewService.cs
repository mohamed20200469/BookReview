using BookReview.Application.DTOs;
using BookReview.Core.Entities;
using BookReview.Core.Interfaces;

namespace BookReview.Application.Services
{
    public class ReviewService
    {
        private readonly IRepository<Review> _reviewRepo;

        public ReviewService(IRepository<Review> reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<List<Review>> GetReviewsByBookId(int bookId)
        {
            var reviews = await _reviewRepo.GetAllAsync();
            var filtered = new List<Review>();
            foreach (var review in reviews)
            {
                if (review.BookId == bookId) filtered.Add(review);
            }
            return filtered;
        }

        public async Task AddReview(ReviewWriteDTO reviewDTO)
        {
            var review = new Review
            {
                BookId = reviewDTO.BookId,
                Text = reviewDTO.Text
            };
            await _reviewRepo.AddAsync(review);
        }

    }
}

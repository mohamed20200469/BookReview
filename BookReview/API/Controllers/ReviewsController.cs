using BookReview.Application.DTOs;
using BookReview.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetReviewsByBookId(int bookId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByBookId(bookId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview(ReviewWriteDTO reviewWriteDTO)
        {
            try
            {
                await _reviewService.AddReview(reviewWriteDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using BookReview.Application.DTOs;
using BookReview.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        private readonly IHttpContextAccessor _http;

        public ReviewsController(ReviewService reviewService, IHttpContextAccessor httpContextAccessor)
        {
            _reviewService = reviewService;
            _http = httpContextAccessor;
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
                var userId = _http.HttpContext?.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
                await _reviewService.AddReview(reviewWriteDTO, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

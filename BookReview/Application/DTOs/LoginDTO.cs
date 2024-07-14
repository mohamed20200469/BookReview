using System.ComponentModel.DataAnnotations;

namespace BookReview.Application.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}

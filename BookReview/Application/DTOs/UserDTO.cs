using System.ComponentModel.DataAnnotations;

namespace BookReview.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}

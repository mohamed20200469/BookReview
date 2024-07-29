using BCrypt.Net;
using BookReview.Application.DTOs;
using BookReview.Core.Entities;
using BookReview.Core.Interfaces;
using System.Text.RegularExpressions;

namespace BookReview.Application.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<User> Register(UserDTO userDTO)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> Login(LoginDTO loginDTO)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(users => users.Email == loginDTO.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<(bool IsValid, string Message)> Validate(UserDTO userDTO)
        {
            return await Task.Run(async () =>
            {
                // No duplicate email
                var users = await _userRepository.GetAllAsync();
                var userX = users.FirstOrDefault(x => x.Email == userDTO.Email);
                if (userX != null)
                {
                    return (false, "Email already in use.");
                }
                // Validate Name
                if (string.IsNullOrWhiteSpace(userDTO.Name))
                {
                    return (false, "Name cannot be empty.");
                }

                var nameParts = userDTO.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length > 3)
                {
                    return (false, "Name can contain at most three parts (first, middle, last).");
                }

                if (!nameParts.All(part => Regex.IsMatch(part, @"^[a-zA-Z]+$")))
                {
                    return (false, "Name can only contain English alphabet characters and spaces.");
                }

                // Validate Email
                if (string.IsNullOrWhiteSpace(userDTO.Email))
                {
                    return (false, "Email cannot be empty.");
                }

                try
                {
                    var addr = new System.Net.Mail.MailAddress(userDTO.Email);
                    if (addr.Address != userDTO.Email)
                    {
                        return (false, "Invalid email format.");
                    }
                }
                catch
                {
                    return (false, "Invalid email format.");
                }

                // Validate Password
                if (string.IsNullOrWhiteSpace(userDTO.Password) || userDTO.Password.Length < 8)
                {
                    return (false, "Password must be at least 8 characters long.");
                }

                if (!userDTO.Password.Any(char.IsDigit))
                {
                    return (false, "Password must contain at least one digit.");
                }

                // If all validations pass
                return (true, "Validation successful.");

            });
        }
    }
}

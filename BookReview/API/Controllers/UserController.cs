using BookReview.Application.DTOs;
using BookReview.Application.Services;
using BookReview.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookReview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserService _userService;

        public UserController(IConfiguration config, UserService userService) 
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var result = await _userService.Validate(userDTO);
            if (!result.IsValid)
            {
                return BadRequest(result.Message);
            }
            var user = await _userService.Register(userDTO);
            var token = await GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userService.Login(loginDTO);
            if (user == null)
            {
                return NotFound("User not found or wrong credentials!");
            }
            Log.Information("User {@user} has logged in successfully!", user);
            var token = await GenerateJwtToken(user);
            return Ok(token);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            return await Task.Run(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["JWT:Secret"]!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, user?.Name!),
                    new Claim(ClaimTypes.NameIdentifier, user?.Id!.ToString()!),
                    new Claim(ClaimTypes.Role, user?.Role!)
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            });
        }
    }
}

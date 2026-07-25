// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokedexApi.Data;
using PokedexApi.DTOs;
using PokedexApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokedexApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            // 1. Check if user or email already exists in static list
            bool userExists = UserStore.Users.Any(u =>
                u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) ||
                u.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase));

            if (userExists)
            {
                return BadRequest(new { message = "Username or Email is already registered." });
            }

            // 2. Hash password manually with BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Create user object
            var newUser = new ApplicationUser
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            // 4. Save to static list
            UserStore.Users.Add(newUser);

            // 5. Generate JWT token
            var token = GenerateJwtToken(newUser);

            return StatusCode(201, new AuthResponseDto
            {
                Message = "Trainer registered successfully!",
                Token = token,
                Username = newUser.Username,
                Email = newUser.Email
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // 1. Find user by Username or Email
            var user = UserStore.Users.FirstOrDefault(u =>
                u.Username.Equals(dto.UsernameOrEmail, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Equals(dto.UsernameOrEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username/email or password." });
            }

            // 2. Verify password with BCrypt
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                return Unauthorized(new { message = "Invalid username/email or password." });
            }

            // 3. Generate JWT Token
            var token = GenerateJwtToken(user);

            return Ok(new AuthResponseDto
            {
                Message = "Login successful!",
                Token = token,
                Username = user.Username,
                Email = user.Email
            });
        }

        // Helper Method to Create JWT Tokens
        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSecret = _configuration["Jwt:Secret"] ?? "SuperSecretKeyWithMinimum32CharactersLength!";
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
// DTOs/AuthDtos.cs
using System.ComponentModel.DataAnnotations;

namespace PokedexApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
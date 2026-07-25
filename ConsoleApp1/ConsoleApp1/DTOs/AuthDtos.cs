using System.ComponentModel.DataAnnotations;

namespace PokedexApi.DTOs
{
    // ==========================================
    // 1. REGISTRATION DTOs
    // ==========================================

    /// <summary>
    /// Request sent by client when registering a new trainer.
    /// </summary>
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }

    // ==========================================
    // 2. LOGIN DTOs
    // ==========================================

    /// <summary>
    /// Request sent by client when logging in.
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

    // ==========================================
    // 3. AUTHENTICATION RESPONSE DTO
    // ==========================================

    /// <summary>
    /// Response returned upon successful registration or login.
    /// </summary>
    public class AuthResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    // ==========================================
    // 4. POKÉDEX DTOs
    // ==========================================

    /// <summary>
    /// Response model returning a user's isolated Pokédex data.
    /// </summary>
    public class UserPokedexDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalCaught { get; set; }
        public List<int> CaughtPokemonIds { get; set; } = new List<int>();
    }
}
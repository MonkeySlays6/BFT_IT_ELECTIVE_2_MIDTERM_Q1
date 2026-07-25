namespace ConsoleApp1.Services;

using ConsoleApp1.DTO;
using ConsoleApp1.Models;

public class UserService
{
    public User RegisterUser(RegisterUserDto dto)
    {
        // 1. Create the User entity
        var newUser = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password) // Always hash passwords!
        };

        // 2. Create the unique UserProfile linked to this user
        newUser.Profile = new UserProfile
        {
            TrainerName = string.IsNullOrEmpty(dto.TrainerName) ? dto.Username : dto.TrainerName,
            FavoritePokemon = string.IsNullOrEmpty(dto.FavoritePokemon) ? "Pikachu" : dto.FavoritePokemon
        };

        // 3. Save to database via DbContext here...
        return newUser;
    }

    private string HashPassword(string password)
    {
        // Placeholder: Use BCrypt, Identity, or Argon2 in production
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
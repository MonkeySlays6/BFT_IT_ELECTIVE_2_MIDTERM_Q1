namespace ConsoleApp1.Services;

using ConsoleApp1.DTO;
using ConsoleApp1.DTOs;
using ConsoleApp1.Models;

public class UserService
{
    // Mock database for testing
    private static readonly List<User> Users = [];

    public User RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User
        {
            Id = Users.Count + 1,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password))
        };

        newUser.Profile = new UserProfile
        {
            Id = newUser.Id,
            UserId = newUser.Id,
            TrainerName = string.IsNullOrWhiteSpace(dto.TrainerName) ? dto.Username : dto.TrainerName,
            FavoritePokemon = string.IsNullOrWhiteSpace(dto.FavoritePokemon) ? "Pikachu" : dto.FavoritePokemon
        };

        Users.Add(newUser);
        return newUser;
    }

    public UserProfileDto? GetUserProfile(int userId)
    {
        var user = Users.FirstOrDefault(u => u.Id == userId);
        if (user is null) return null;

        return new UserProfileDto
        {
            UserId = user.Id,
            Username = user.Username,
            TrainerName = user.Profile.TrainerName,
            FavoritePokemon = user.Profile.FavoritePokemon,
            AvatarUrl = user.Profile.AvatarUrl
        };
    }
}
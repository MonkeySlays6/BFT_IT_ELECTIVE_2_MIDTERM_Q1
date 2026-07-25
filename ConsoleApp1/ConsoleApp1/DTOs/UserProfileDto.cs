namespace ConsoleApp1.DTOs;

public class UserProfileDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public string FavoritePokemon { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}
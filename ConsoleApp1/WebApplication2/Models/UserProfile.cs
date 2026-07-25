namespace ConsoleApp1.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public string FavoritePokemon { get; set; } = "Pikachu";
    public string AvatarUrl { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    // Foreign key back to User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
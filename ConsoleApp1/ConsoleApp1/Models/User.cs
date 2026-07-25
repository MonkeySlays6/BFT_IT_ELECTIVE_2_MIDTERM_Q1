namespace ConsoleApp1.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Navigation property: One user has exactly one profile
    public UserProfile Profile { get; set; } = null!;
}
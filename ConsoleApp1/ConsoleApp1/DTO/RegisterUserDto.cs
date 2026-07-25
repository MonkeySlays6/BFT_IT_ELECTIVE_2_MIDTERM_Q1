namespace ConsoleApp1.DTO;

public class RegisterUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public string FavoritePokemon { get; set; } = string.Empty;
}
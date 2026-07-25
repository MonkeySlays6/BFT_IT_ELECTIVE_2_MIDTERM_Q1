// Models/ApplicationUser.cs
namespace PokedexApi.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<int> CaughtPokemonIds { get; set; } = new List<int>();
    }
}
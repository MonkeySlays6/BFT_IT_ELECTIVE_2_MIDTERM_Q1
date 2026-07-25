namespace WebApplication2.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Pokemon
    {
        public string Name { get; set; }
        public string Url { get; set; }

        // New properties for our UI upgrade
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PokeApiResponse
    {
        public List<Pokemon> Results { get; set; }
    }

    public static class AppData
    {
        public static List<User> Users = new List<User>
        {
            new User { Username = "Ash", Password = "password123" },
            new User { Username = "Misty", Password = "waterpokemon" }
        };

        public static List<Pokemon> AllPokemon = new List<Pokemon>();

        // NEW: A dictionary to act as our database for caught Pokemon.
        // It ties a username to a list of their caught Pokemon!
        public static Dictionary<string, List<Pokemon>> CaughtPokemon = new Dictionary<string, List<Pokemon>>();
    }
}
namespace WebApplication1.Models
{
    public class PokeApiResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int height { get; set; }
        public int weight { get; set; }

        public Sprites sprites { get; set; }
        public List<TypeSlot> types { get; set; }
    }

    public class Sprites
    {
        public string front_default { get; set; }
    }

    public class TypeSlot
    {
        public PokemonType type { get; set; }
    }

    public class PokemonType
    {
        public string name { get; set; }
    }
}

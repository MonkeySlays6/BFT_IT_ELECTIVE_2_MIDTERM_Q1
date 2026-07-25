public class Pokemon
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public List<string> Types { get; set; } = new();

    public int Height { get; set; }

    public int Weight { get; set; }
}
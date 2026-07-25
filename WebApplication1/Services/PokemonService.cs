using System.Net.Http.Json;
using WebApplication1.Models;


public class PokemonService
{
    private readonly HttpClient _httpClient;

    public PokemonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Pokemon?> GetPokemon(string name)
    {
        var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(
            $"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}");

        if (response == null)
            return null;

        return new Pokemon
        {
            Id = response.id,
            Name = response.name,
            Image = response.sprites.front_default,
            Height = response.height,
            Weight = response.weight,
            Types = response.types
                .Select(t => t.type.name)
                .ToList()
        };
    }

    public async Task CatchPokemon(string username, string pokemonName)
    {
        var pokemon = await GetPokemon(pokemonName);

        if (pokemon == null)
            return;

        StaticData.UserPokemons.Add(new UserPokemon
        {
            Username = username,
            Pokemon = pokemon,
            DateCaught = DateTime.Now
        });
    }
    public void ReleasePokemon(string username, int pokemonId)
    {
        var pokemon = StaticData.UserPokemons
            .FirstOrDefault(x =>
                x.Username == username &&
                x.Pokemon.Id == pokemonId);

        if (pokemon != null)
        {
            StaticData.UserPokemons.Remove(pokemon);
        }
    }

    public List<Pokemon> GetUserPokemon(string username)
    {
        return StaticData.UserPokemons
            .Where(x => x.Username == username)
            .Select(x => x.Pokemon)
            .ToList();
    }

}
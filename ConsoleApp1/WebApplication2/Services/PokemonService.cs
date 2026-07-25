using System.Net.Http.Json;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Pokemon?> GetPokemon(string name)
        {
            var response = await _httpClient.GetFromJsonAsync<PokeDetailResponse>(
                $"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}");

            if (response == null)
                return null;

            return new Pokemon
            {
                Id = response.id,
                Name = response.name,
                ImageUrl = response.sprites.front_default
            };
        }

        public async Task CatchPokemon(string username, string pokemonName)
        {
            var pokemon = await GetPokemon(pokemonName);

            if (pokemon == null)
                return;

            else if (!AppData.CaughtPokemon.ContainsKey(username))
            {
                AppData.CaughtPokemon[username] = new List<Pokemon>();
            }

            AppData.CaughtPokemon[username].Add(pokemon);
        }
        public void ReleasePokemon(string username, int pokemonId)
        {
            var pokemon = AppData.CaughtPokemon[username]
                .FirstOrDefault(x => x.Id == pokemonId);

            if (pokemon != null)
            {
                AppData.CaughtPokemon[username].Remove(pokemon);
            }
        }

        public List<Pokemon> GetUserPokemon(string username)
        {
            return AppData.CaughtPokemon.ContainsKey(username)
                ? AppData.CaughtPokemon[username]
                : new List<Pokemon>();
        }

    }
}
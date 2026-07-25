using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Added searchString parameter
        public async Task<IActionResult> Index(string searchString)
        {
            // 1. Fetch data from PokeAPI if our static list is empty
            if (AppData.AllPokemon.Count == 0)
            {
                var response = await _httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=1025");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var apiData = JsonSerializer.Deserialize<PokeApiResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiData != null && apiData.Results != null)
                    {
                        foreach (var p in apiData.Results)
                        {
                            var urlSegments = p.Url.TrimEnd('/').Split('/');
                            if (int.TryParse(urlSegments.Last(), out int id))
                            {
                                p.Id = id;
                                p.ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
                            }
                        }
                        AppData.AllPokemon = apiData.Results;
                    }
                }
            }

            // 2. Filter the list if the user typed something in the search bar
            var displayPokemon = AppData.AllPokemon;

            if (!string.IsNullOrEmpty(searchString))
            {
                // Convert to lowercase since PokeAPI names are lowercase
                displayPokemon = displayPokemon.Where(p => p.Name.Contains(searchString.ToLower())).ToList();
            }

            // 3. Send the filtered list to the view
            return View(displayPokemon);
        }

        // NEW: Action to handle the Catch button press
        [HttpPost]
        public IActionResult Catch(string pokemonName)
        {
            var username = User.Identity.Name;

            // Ensure the user has an inventory list created in our static dictionary
            if (!AppData.CaughtPokemon.ContainsKey(username))
            {
                AppData.CaughtPokemon[username] = new List<Pokemon>();
            }

            // Find the pokemon they clicked and add it to their inventory
            var caughtMon = AppData.AllPokemon.FirstOrDefault(p => p.Name == pokemonName);
            if (caughtMon != null)
            {
                AppData.CaughtPokemon[username].Add(caughtMon);
            }

            // Send them back to the main Pokedex screen
            return RedirectToAction("Index");
        }

        // NEW: Action to view the Inventory page
        public IActionResult Inventory()
        {
            var username = User.Identity.Name;

            // Get this specific user's caught pokemon, or an empty list if they haven't caught any yet
            var myPokemon = AppData.CaughtPokemon.ContainsKey(username)
                ? AppData.CaughtPokemon[username]
                : new List<Pokemon>();

            return View(myPokemon);
        }
    }
}
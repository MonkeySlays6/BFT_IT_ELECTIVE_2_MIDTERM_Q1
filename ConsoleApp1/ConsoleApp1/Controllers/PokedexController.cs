// Controllers/PokedexController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokedexApi.Data;
using System.Security.Claims;

namespace PokedexApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires valid JWT token header
    public class PokedexController : ControllerBase
    {
        // Helper method to extract current user ID from the JWT token
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: api/pokedex/my-pokedex
        // Returns ONLY the logged-in user's Pokédex
        [HttpGet("my-pokedex")]
        public IActionResult GetMyPokedex()
        {
            var userId = GetUserId();
            var user = UserStore.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            return Ok(new
            {
                username = user.Username,
                caughtCount = user.CaughtPokemonIds.Count,
                caughtPokemonIds = user.CaughtPokemonIds
            });
        }

        // POST: api/pokedex/catch/25
        // Adds a Pokémon to ONLY this user's Pokédex
        [HttpPost("catch/{pokemonId}")]
        public IActionResult CatchPokemon(int pokemonId)
        {
            var userId = GetUserId();
            var user = UserStore.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            // Prevent duplicate entries for the same Pokémon
            if (!user.CaughtPokemonIds.Contains(pokemonId))
            {
                user.CaughtPokemonIds.Add(pokemonId);
            }

            return Ok(new
            {
                message = $"Pokémon #{pokemonId} added to {user.Username}'s Pokédex!",
                caughtPokemonIds = user.CaughtPokemonIds
            });
        }

        // DELETE: api/pokedex/release/25
        // Removes a Pokémon from ONLY this user's Pokédex
        [HttpDelete("release/{pokemonId}")]
        public IActionResult ReleasePokemon(int pokemonId)
        {
            var userId = GetUserId();
            var user = UserStore.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            user.CaughtPokemonIds.Remove(pokemonId);

            return Ok(new
            {
                message = $"Pokémon #{pokemonId} released from {user.Username}'s Pokédex.",
                caughtPokemonIds = user.CaughtPokemonIds
            });
        }
    }
}
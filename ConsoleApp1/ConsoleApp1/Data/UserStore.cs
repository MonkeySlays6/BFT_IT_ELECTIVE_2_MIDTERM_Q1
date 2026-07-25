// Data/UserStore.cs
using PokedexApi.Models;
using System.Collections.Concurrent;

namespace PokedexApi.Data
{
    public static class UserStore
    {
        // Thread-safe list to store registered users in memory
        public static ConcurrentBag<ApplicationUser> Users { get; } = new ConcurrentBag<ApplicationUser>();
    }
}
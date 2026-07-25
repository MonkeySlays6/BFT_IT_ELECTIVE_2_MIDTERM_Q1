using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Check static list and require password
            var user = AppData.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // This acts as the "Switch Account" feature
        // This is your new Log Out feature!
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
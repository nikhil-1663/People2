using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using People2.Data;
using System.Security.Claims;
using People2.Models;
using Microsoft.EntityFrameworkCore;

namespace YourProjectName.Controllers
{
    public class Authentication : Controller
    {
        private readonly ApplicationDbContext _context;

        public Authentication(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login Page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginMod model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return view if validation fails
            }

            // Check if user exists in the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);

            if (user == null || user.Password != model.Password) // Plaintext password check (replace with hashed check)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            // Correct authentication using ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("UserId", user.Id.ToString())
    }, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Index", "Person"); // Redirects to the Person List Page
        }



        // Logout Action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using YoKart.Models;
using YoKart.IServices;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;
using System.Data;


namespace YoKart.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _service;

        public UserController(ILogger<UserController> logger, IUserServices service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser _login)
        {
            if (ModelState.IsValid)
            {
                var token = await _service.ValidateUser(_login);
                if (token == null)
                {
                    ModelState.AddModelError("Username", "Incorrect username. Please try again.");
                    ModelState.AddModelError("Password", "Incorrect password. Please try again.");
                    return View();
                }

                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, HttpContext.Session.GetString("UserName"))
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

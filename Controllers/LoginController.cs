using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserServices _service;

        public LoginController(ILogger<LoginController> logger, IUserServices service)
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
                var data = await _service.ValidateUser(_login);
                if (data == (null, null,null))
                {
                    ModelState.AddModelError("Username", "Incorrect username. Please try again.");
                    ModelState.AddModelError("Password", "Incorrect password. Please try again.");
                    return View();
                }
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, data.Item1, data.Item2);

                myVar.Roles = data.Item3.Roles;

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
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

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using YoKart.Models;

namespace YoKart.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _client = new HttpClient();
        private readonly string url = "https://localhost:44373/api/UserApi";

        public LoginController(ILogger<LoginController> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUser _login)
        {
            if (ModelState.IsValid)
            {
                var data = JsonConvert.SerializeObject(_login);
                StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                using (var response = _client.PostAsync(url, stringContent).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = response.Content.ReadAsStringAsync().Result;
                        if (errorContent.Contains("System.Exception: User is not valid"))
                        {
                            ModelState.AddModelError("Username", "Incorrect username. Please try again.");
                            ModelState.AddModelError("Password", "Incorrect password. Please try again.");
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
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

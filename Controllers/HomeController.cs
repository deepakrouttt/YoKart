using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using YoKart.IServices;
using YoKart.Models;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public readonly HttpClient _client;
        public readonly ICategoryServices _serviceCat;

        public HomeController(HttpClient client, ICategoryServices serviceCat)
        {
            _serviceCat = serviceCat;
            _client = client;
        }
       
    public async Task<IActionResult> Index()
        {
            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));
            var response = await _client.GetAsync(url);
            var products = new List<Product>();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);
                if (data != null)
                {
                    products = data;
                    return View(products);
                }
            }
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> ProductIndex(int id)
        {
            var url = "https://localhost:44373/api/ProductApi/" + id;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));
            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                if (result != null)
                    return View(result);
                
            }
            return RedirectToAction("Login", "User");
        }
        public IActionResult privacy()
        {
            return View();
        }

    }
}

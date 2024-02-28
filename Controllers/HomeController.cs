using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public readonly HttpClient _client = new HttpClient();
        public async Task<IActionResult> Index()
        {
            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            var response =await _client.GetAsync(url);
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
            return View();
        }
        public IActionResult privacy()
        {
            return View();
        }

    }
}

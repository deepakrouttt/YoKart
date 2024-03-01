using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public readonly HttpClient _client = new HttpClient();
        public readonly ICategoryServices _serviceCat;

        public HomeController(HttpClient client, ICategoryServices serviceCat)
        {
            _client = client;
            _serviceCat = serviceCat;
        }

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

        public async Task<IActionResult> ProductIndex(int id)
        {
            var _cate = await _serviceCat.CategoryList();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var url = "https://localhost:44373/api/ProductApi/" + id;
            var response = _client.GetAsync(url).Result;

            if(response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

                if(result != null)
                {
                    return View(result);
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

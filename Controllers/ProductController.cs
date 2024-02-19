using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;
using YoKart.Services;
using static System.Net.WebRequestMethods;

namespace YoKart.Controllers
{
    public class ProductController : Controller
    {
        public readonly ICategoryItem _data;
        public readonly HttpClient _client;

        public ProductController(ICategoryItem data, HttpClient client)
        {
            _data = data;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            var response = _client.GetAsync(url).Result;
            var product = new List<Product>();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);

                if (data != null)
                {
                    product = data;

                }
                return View(product);
            }

            return View();
        }

        public async Task<IActionResult> Create()
        {
            var data = await _data.CategoryData();
            ViewData["categories"] = data.CategoryList;
            ViewData["subcategories"] = data.SubCategoryList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var url = "https://localhost:44373/api/ProductApi/AddProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var url = "https://localhost:44373/api/ProductApi/" + id;
            var response = _client.GetAsync(url).Result;
            var product = new Product();

            if (response.IsSuccessStatusCode)
            {
                var result =await  response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Product>(result);

                if (data != null)
                {
                    product = data;

                    return View(product);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var url = "https://localhost:44373/api/ProductApi/UpdateProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            var response = _client.PostAsync(url,stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

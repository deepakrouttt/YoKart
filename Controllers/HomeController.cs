using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            var client = new HttpClient();
            var _url = "https://localhost:44373/api/CategoryApi/addCategories";

            var categoryWithSubCategories = new
            {
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");

            var response = client.PostAsync(_url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }


            return View("Create", category);
        }



    }
}

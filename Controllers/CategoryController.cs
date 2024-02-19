using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class CategoryController : Controller
    {
        public HttpClient _client = new HttpClient();
        public readonly ICategoryItem _data;

        public CategoryController(HttpClient client, ICategoryItem data)
        {
            _client = client;
            _data = data;
        }

        public async Task<IActionResult> Index()
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
            var _url = "https://localhost:44373/api/CategoryApi/addCategories";
            var categoryWithSubCategories = new
            {
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }


            return View("Create", category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
            var response = await _client.GetAsync(apiUrl);

            var category = new Category();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Category>(result);

                if (data != null)
                {
                    category = data;

                    return View(category);
                }
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var url = "https://localhost:44373/api/CategoryApi/editCategories";
            var categoryWithSubCategories = new
            {
                categoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", category);
        }

        [HttpGet]
        public async Task<IActionResult> Exist(int id)
        {

            var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
            var response = await _client.GetAsync(apiUrl);

            var category = new Category();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Category>(result);

                if (data != null)
                {
                    category = data;

                    return View(category);
                }
            }
            return View();

        }

        [HttpPost]
        public IActionResult Exist(Category category)
        {
            var url = "https://localhost:44373/api/CategoryApi/existCategories";
            var categoryWithSubCategories = new
            {
                categoryId = (GlobalVariable.categoryId == null) ? category.CategoryId : GlobalVariable.categoryId,
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                GlobalVariable.categoryId = null;
                return RedirectToAction("Index");
            }

            return View("Edit", category);
        }

        [HttpPost]
        public IActionResult ExistCategory(Category category)
        {
            var url = "https://localhost:44373/api/CategoryApi/existCategories";

            var categoryWithSubCategories = new
            {
                categoryId = (GlobalVariable.categoryId == null) ? category.CategoryId : GlobalVariable.categoryId,
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", category);
        }

        [HttpGet]
        public async Task<IActionResult> IndexSub(int id)
        {
                var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
                var response = await _client.GetAsync(apiUrl);

                var category = new Category();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Category>(result);

                    if (data != null)
                    {
                        category = data;

                        return View(category);
                    }
                }
                return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> EditSub(int id)
        {
 
                var apiUrl = $"https://localhost:44373/api/CategoryApi/GetSubCategory/{id}";
                var response = await _client.GetAsync(apiUrl);

                var subcategory = new SubCategory();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<SubCategory>(result);

                    if (data != null)
                    {
                        subcategory = data;

                        return View(subcategory);
                    }
                }
                return View();
            
        }
        [HttpPost]
        public IActionResult EditSub(SubCategory category)
        {
            var url = "https://localhost:44373/api/CategoryApi/existSubCategories";

            var categoryWithSubCategories = new
            {
                SubCategoryId = category.SubCategoryId,
                SubCategoryName = category.SubCategoryName,
                CategoryId = category.CategoryId
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Category");
            }

            return View("EditSsub", category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var url = $"https://localhost:44373/api/CategoryApi/removeCategoryies?id={id}";
            var response = _client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Index");

        }
        [HttpGet]
        public IActionResult DeleteSub(int id)
        {
            var url = $"https://localhost:44373/api/CategoryApi/removeSubCategoryies?id={id}";
            var response = _client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("IndexSub");
            }

            return View("Index");

        }

        [HttpPost]
        public IActionResult SavedCategoryId(int categoryId)
        {
            GlobalVariable.categoryId = categoryId;
            return Json(new { success = true });
        }



    }
}

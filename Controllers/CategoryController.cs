﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class CategoryController : Controller
    {
        public HttpClient _client = new HttpClient();
        public readonly ICategoryServices _service;

        public CategoryController(HttpClient client, ICategoryServices data)
        {
            _client = client;
            _service = data;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var _cate = await _service.CategoryData();
            var category = _cate.CategoryList;

            var tempCategory = myVar.PagingCategory(category, page);

            return View("Index", tempCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Index_Partial(int? page)
        {
            var _cate = await _service.CategoryData();
            var category = _cate.CategoryList;

            var tempCategory = myVar.PagingCategory(category, page);

            return PartialView("_Index", tempCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {

            var response = await _service.Create(category);

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

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", category);
        }

        [HttpPost]
        public async Task<IActionResult> Exist(Category category)
        {
            var response = await _service.Exist(category);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            return View("Edit", category);
        }

        [HttpGet]
        public async Task<IActionResult> IndexSub(int id,int? page)
        {
            var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
            var response = await _client.GetAsync(apiUrl);

            var subCategory = new Category();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Category>(result);
                if (data != null)
                {
                    subCategory = myVar.PagingSubCategory(data,page);   
                    return View("IndexSub",subCategory);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexSub_Partial(int id,int? page)
        {
            var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
            var response = await _client.GetAsync(apiUrl);

            var subCategory = new Category();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Category>(result);
                if (data != null)
                {
                    subCategory = myVar.PagingSubCategory(data, page);
                }
            }     
            return PartialView("_IndexSub", subCategory);
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

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;
            var cateid = category.SubCategoryId;

            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/Category/IndexSub/" + category.CategoryId);
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
                return RedirectToAction("Index");
            }

            return View("Index");

        }

    }
}

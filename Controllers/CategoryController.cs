﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    [Authorize]
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
            var _cate = await _service.CategoryList();
            var category = _cate.CategoryList;
            var tempCategory = myVar.PagingCategory(category, page);
            return View("Index", tempCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Index_Partial(int? page)
        {
            var _cate = await _service.CategoryList();
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
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View("Create", category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.Edit(id);
            if (category != null) { return View(category); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var response = await _service.Edit(category);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View("Edit", category);
        }

        [HttpPost]
        public async Task<IActionResult> Exist(Category category)
        {
            var response = await _service.Exist(category);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View("Edit", category);
        }

        public async Task<IActionResult> IndexSub(int? id, int? page)
        {
            var subCategory = await _service.IndexSub(id, page);
            if (subCategory != null) { return View("IndexSub", subCategory); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexSub_Partial(int? id, int? page)
        {
            var subCategory = await _service.IndexSub(id, page);
            return PartialView("_IndexSub", subCategory);
        }
        [HttpGet]
        public async Task<IActionResult> EditSub(int id)
        {
            var subcategory = await _service.EditSub(id);
            if (subcategory != null) { return View(subcategory); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditSub(SubCategory category)
        {
            var response = await _service.EditSub(category);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/Category/IndexSub/" + category.CategoryId);
            }
            return View("EditSsub", category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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

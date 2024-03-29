﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YoKart.IServices;
using YoKart.Models;

namespace YoKart.Services
{
    public class CategoryServices : ICategoryServices
    {
        HttpClient _client = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string baseUrl = "https://localhost:44373/api/CategoryApi/";

        public CategoryServices(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> Create(Category category)
        {
            var _url = $"{baseUrl}addCategories";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_url, stringContent);

            return response;
        }

        public async Task<HttpResponseMessage> Exist(Category category)
        {

            var url = $"{baseUrl}existCategories";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var categoryWithSubCategories = new
            {
                categoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories.Select(sub => new { SubCategoryName = sub.SubCategoryName }).ToList()
            };
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(categoryWithSubCategories), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, stringContent);

            return response;
        }

        public async Task<Category> Edit(int id)
        {
            var apiUrl = $"{baseUrl}{id}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response =  _client.GetAsync(apiUrl).Result;
            var category = new Category();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Category>(result);

                if (data != null)
                {
                    category = data;
                }
            }
            return category;
        }

        public async Task<HttpResponseMessage> Edit(Category category)
        {
            var url = $"{baseUrl}editCategories";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(url, stringContent).Result;
            return response;
        }

        public async Task<Category> IndexSub(int? id, int? page)
        {
            var apiUrl = $"{baseUrl}{id}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
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
            return subCategory;
        }

        public async Task<SubCategory> EditSub(int id)
        {

            var apiUrl = $"{baseUrl}GetSubCategory/{id}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = await _client.GetAsync(apiUrl);

            var subcategory = new SubCategory();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<SubCategory>(result);

                if (data != null)
                {
                    subcategory = data;
                }
            }
            return subcategory;
        }

        public async Task<HttpResponseMessage> EditSub(SubCategory category)
        {
            var url = $"{baseUrl}existSubCategories";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, stringContent);
            return response;
        }
    }
}
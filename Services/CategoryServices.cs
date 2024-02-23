using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;

namespace YoKart.Services
{
    public class CategoryServices : ICategoryServices
    {
        HttpClient _client = new HttpClient();

        public CategoryServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<CategoriesView> CategoryList()
        {
            var httpClient = new HttpClient();
            var apiUrl = "https://localhost:44373/api/CategoryApi/";

            var categoriesResponse = await httpClient.GetAsync(apiUrl + "categories");
            var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

            var subcategoriesResponse = await httpClient.GetAsync(apiUrl + "subcategories");
            var subcategoriesJson = await subcategoriesResponse.Content.ReadAsStringAsync();
            var subcategories = JsonConvert.DeserializeObject<List<SubCategory>>(subcategoriesJson);

            return new() { CategoryList = categories, SubCategoryList = subcategories };
        }

        public async Task<HttpResponseMessage> Create(Category category)
        {
            var _url = "https://localhost:44373/api/CategoryApi/addCategories";

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_url, stringContent);

            return response;
        }

        public async Task<HttpResponseMessage> Exist(Category category)
        {

            var url = "https://localhost:44373/api/CategoryApi/existCategories";
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
            var apiUrl = $"https://localhost:44373/api/CategoryApi/{id}";
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
            var url = "https://localhost:44373/api/CategoryApi/editCategories";

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            return response;
        }

        public async Task<Category> IndexSub(int? id, int? page)
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
            return subCategory;
        }

        public async Task<SubCategory> EditSub(int id)
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
                }
            }
            return subcategory;
        }

        public async Task<HttpResponseMessage> EditSub(SubCategory category)
        {
            var url = "https://localhost:44373/api/CategoryApi/existSubCategories";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, stringContent);
            return response;
        }
    }
}
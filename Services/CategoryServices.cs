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

        public async Task<CategoriesView> CategoryData()
        {
            var httpClient = new HttpClient();
            var apiUrl = "https://localhost:44373/api/CategoryApi/";

            var categoriesResponse = await httpClient.GetAsync(apiUrl + "categories");
            var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

            var subcategoriesResponse = await httpClient.GetAsync(apiUrl + "subcategories");
            var subcategoriesJson = await subcategoriesResponse.Content.ReadAsStringAsync();
            var subcategories = JsonConvert.DeserializeObject<List<SubCategory>>(subcategoriesJson);

            return new (){ CategoryList = categories, SubCategoryList = subcategories };
        }

        public async Task<HttpResponseMessage>Create(Category category)
        {
            var _url = "https://localhost:44373/api/CategoryApi/addCategories";

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response =await  _client.PostAsync(_url, stringContent);

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
            var response =await  _client.PutAsync(url, stringContent);

            return response;
        }
    }
}

using Newtonsoft.Json;
using YoKart.Models;

namespace YoKart.Services
{
    public class CategoryServices : ICategoryServices
    {
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
    }
}

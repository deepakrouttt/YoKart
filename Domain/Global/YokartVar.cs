
using Domain.Models;

namespace Domain.Global
{
    public static class YokartVar
    {
        //CategoriesList
        public static List<Category> categories = new List<Category>();
        public static List<SubCategory> subcategories = new List<SubCategory>();
        //paging
        public static List<string> imagePaths = new List<string>();
        public static int pageSize { get; set; }
        public static int pageCount { get; set; }
        public static int totalProduct { get; set; }
        public static int currentPage { get; set; }

        //paging 
        public static List<Category> PagingCategory(List<Category> categories, int? page)
        {
            pageSize = 3;
            pageCount = (int)Math.Ceiling(categories.Count / (double)pageSize);
            currentPage = page ?? 1;
            var tempCategory = categories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return tempCategory;
        }

        public static Category PagingSubCategory(Category category, int? page)
        {
            pageSize = 3;
            pageCount = (int)Math.Ceiling(category.SubCategories.Count / (double)pageSize);
            currentPage = page ?? 1;
            var tempSubCategory = category.SubCategories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var tempCategory = new Category
            {
                CategoryId = category.CategoryId,
                SubCategories = tempSubCategory
            };

            return tempCategory;
        }   
    }
}

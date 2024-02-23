using Microsoft.AspNetCore.Mvc.RazorPages;
using YoKart.Models;

namespace YoKart
{
    public static class myVar
    {
        public static List<string> imagePaths = new List<string>();
        public static int pageSize = 2;
        public static int pageCount { get; set; }
        public static int Total { get; set; }
        public static int currentPage { get; set; }

        //paging 
        public static List<Category> PagingCategory(List<Category> categories, int? page)
        {
            pageCount = (int)Math.Ceiling(categories.Count / (double)pageSize);
            currentPage = page ?? 1;
            var tempCategory = categories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return tempCategory;
        }

        public static Category PagingSubCategory(Category category, int? page)
        {
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

        public static List<Product> PagingProduct(IEnumerable<Product> products,int? page)
        {
            var productList = products.ToList();
            pageCount =(int) Math.Ceiling(productList.Count / (double)pageSize);

            currentPage = page ?? 1;
            var tempProduct = products.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return tempProduct;
        }

    }
}

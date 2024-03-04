using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using YoKart.Models;

namespace YoKart
{
    public static class myVar
    {
        public static List<string> imagePaths = new List<string>();
        public static int pageSize { get; set; }
        public static int pageCount { get; set; }
        public static int totalProduct { get; set; }
        public static int currentPage { get; set; }
        public static int UserId { get; set; }
        public static String  Roles { get; set; }
        public static String UserName { get; set; }

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

        public static string NumberFormeting(decimal price)
        {   
            string formatted = price.ToString("#,##0.00");
            return formatted;
        }

        public static List<string> DiscriptionFormat(string discription)
        {
            var ListDiscription = discription.Split(',').ToList();
            return ListDiscription;
        }

        public static void userDetails(User user)
        {
            UserId = user.Id;
            Roles = user.Roles;
            UserName = user.Username;
        }
    }
}


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
        public static int pageSize { get; set; } = 3;
        public static int pageCount { get; set; }
        public static int totalProduct { get; set; }
        public static int currentPage { get; set; }

        //paging 
 
 
    }
}

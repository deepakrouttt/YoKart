using System.ComponentModel.DataAnnotations;

namespace YoKart.Models
{
    public class Category
    {
        [Required]
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public List<SubCategory> SubCategories { get; set; }

    }
    public class CategoriesView
    {
        public List<Category> CategoryList;
        public List<SubCategory> SubCategoryList;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Domain.Models
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
}

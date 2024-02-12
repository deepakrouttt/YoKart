using System.ComponentModel.DataAnnotations;

namespace YoKart.Models
{
    public class SubCategory
    {
        [Required]
        [Key]
        public int SubCategoryId { get; set; }

        [Required]
        public string SubCategoryName { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoKart.Models
{
    public class Product
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public String ProductImage { get; set; }
        [Required]
        public IFormFile? ProductImageFile { get; set; }
        [Required]
        public string ProductPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
    }
    public class Paging
    {
        public int? page { get; set; }
        public long LowRange { get; set; }
        public long HighRange { get; set; }
    }

    public class ProductPagingData
    {
        public List<Product> Product { get; set; }
        public int pageSize = 2;
        public int pageCount { get; set; }
        public int Total { get; set; }
        public int currentPage { get; set; }
    }
    public class ProductUpdate
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public String ProductImage { get; set; }
        [Required]
        public string ProductPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
    }
}

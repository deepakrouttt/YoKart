using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
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
        [NotMapped]
        public IFormFile? ProductImageFile { get; set; }
        [Required]
        [RegularExpression(@"^\d{0,8}(\.\d{1,4})?$", ErrorMessage = "Please enter a valid product price.")]
        public Decimal ProductPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
    }
    //paging class 
    public class filtering
    {
        public int page { get; set; } = 1;
        public decimal LowRange { get; set; }
        public decimal HighRange { get; set; }
        public string? Sort { get; set; }
    }
    //Api class for fetch the data
    public class ProductPagingData
    {
        public List<Product> Product { get; set; }
        public int pageSize = 2;
        public int pageCount { get; set; }
        public int totalProduct { get; set; }
        public int currentPage { get; set; }
    }
    //Api product type
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
        public decimal ProductPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
    }
}

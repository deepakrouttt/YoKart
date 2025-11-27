using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class OrderItem
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Products { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        [Required]
        public decimal Price => (Products?.ProductPrice ?? decimal.Zero) * Quantity;
    }

    public class Order
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public String OrderStatus { get; set; }

        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }

        [Required]
        public decimal? TotalPrice
        {
            get
            {
                return OrderItems?.Sum(item => item.Price);
            }
        }

        public class OrderDetails
        {
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public String OrderStatus { get; set; }
        }
    }
}

using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using static Domain.Models.Order;

namespace Application.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly YoKartDbContext _context;

        public OrderRepo(YoKartDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddProductToOrder(OrderDetails orderDetails)
        {

            var existingOrder = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
               SingleOrDefault(o => o.UserId == orderDetails.UserId && o.OrderStatus == "Cart");

            if (existingOrder == null)
            {
                {
                    var order = new Order()
                    {
                        UserId = orderDetails.UserId,
                        OrderStatus = "Cart"
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    existingOrder = _context.Orders
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Products)
                        .Single(o => o.UserId == orderDetails.UserId && o.OrderStatus == "Cart");
                }
            }

            if (existingOrder != null)
            {
                var existingProduct = _context.Products.Find(orderDetails.ProductId);
                if (existingProduct != null)
                {
                    var newOrderItem = new OrderItem
                    {
                        ProductId = orderDetails.ProductId,
                        UnitPrice = existingProduct.ProductPrice,
                        Quantity = orderDetails.Quantity,
                        OrderDate = DateTime.Now,
                        Products = existingProduct
                    };
                    existingOrder.OrderStatus = orderDetails.OrderStatus;
                    existingOrder.OrderItems.Add(newOrderItem);
                    _context.SaveChanges();
                }
            }
            return existingOrder;
        }

        public async Task<OrderItem> UpdateOrder(OrderDetails orderDetails)
        {
            var existingOrder = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
              SingleOrDefault(o => o.UserId == orderDetails.UserId && o.OrderStatus == "Cart");

            if (existingOrder != null)
            {
                var existingProduct = existingOrder.OrderItems?.FirstOrDefault(m => m.ProductId == orderDetails.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.LastUpdateDate = DateTime.Now;
                    existingProduct.Quantity = orderDetails.Quantity;
                }
                _context.OrderItems.Update(existingProduct);
                _context.SaveChanges();
                return existingProduct;
            }
            return null;
        }

        public async Task<OrderItem> RemoveProductToOrder(int UserId, int ProductId)
        {
            var existingOrder = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
              SingleOrDefault(o => o.UserId == UserId && o.OrderStatus == "Cart");

            if (existingOrder != null)
            {
                var existingProduct = existingOrder.OrderItems.FirstOrDefault(m => m.ProductId == ProductId);

                existingOrder.OrderItems.Remove(existingProduct);
                _context.OrderItems.Remove(existingProduct);
                _context.SaveChanges();

                if (existingOrder.OrderItems.Count == 0)
                {
                    _context.Orders.Remove(existingOrder);
                    _context.SaveChanges();
                    return existingProduct;
                }

                return existingProduct;
            }
            return null;
        }

        public async Task<bool> OrderPlaced(int UserId)
        {
            var existingOrder = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
              SingleOrDefault(o => o.UserId == UserId && o.OrderStatus == "Cart");

            if (existingOrder != null)
            {
                existingOrder.OrderStatus = "Order Placed";
                _context.Orders.Update(existingOrder);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}

using Domain.Mail;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
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
        public async Task<List<Order>> GetOrders()
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).Where(o => o.OrderItems.
            Any(oi => oi.Products.ProductId == oi.ProductId)).ToList();

            return orders;
        }
        public async Task<Order> GetOrders(int id)
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
          SingleOrDefault(o => o.UserId == id && o.OrderStatus == "Cart");

            return orders;
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

        //public async Task<bool> Checkout(int UserId)
        //{
        //    var url = $"https://localhost:44373/api/UserApi/{UserId}";
        //    var response = await _client.GetAsync(url);
        //    var mailMessage = false;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        var user = JsonConvert.DeserializeObject<User>(result);
        //        var order = JsonConvert.SerializeObject(await Index(UserId));

        //        var PlacedUrl = $"{baseUrl}OrderPlaced";
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
        //        var content = new StringContent(JsonConvert.SerializeObject(UserId), Encoding.UTF8, "application/json");
        //        var responseOrder = await _client.PostAsync(PlacedUrl, content);

        //        if (user != null && responseOrder.IsSuccessStatusCode)
        //        {
        //            var mailData = new MailData()
        //            {
        //                EmailToId = user.Email,
        //                EmailToName = user.Firstname + " " + user.Lastname,
        //                EmailSubject = "Order Placed",
        //                EmailBody = order
        //            };
        //            var mailUrl = "https://localhost:44373/api/MailApi/SendMail";
        //            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(mailData), Encoding.UTF8, "application/json");
        //            var mailResponse = await _client.PostAsync(mailUrl, stringContent);
        //            var mailResult = await mailResponse.Content.ReadAsStringAsync();
        //            mailMessage = JsonConvert.DeserializeObject<bool>(mailResult);
        //        }
        //    }
        //    return mailMessage;
        //}
    }
}

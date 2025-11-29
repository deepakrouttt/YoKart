using Domain.Mail;
using Domain.Models;
using static Domain.Models.Order;

namespace Infrastructure.IRepository
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrders(int id);
        Task<Order> AddProductToOrder(OrderDetails orderDetails);
        Task<OrderItem> UpdateOrder(OrderDetails orderDetails);
        Task<OrderItem> RemoveProductToOrder(int UserId, int ProductId);
        Task<bool>  OrderPlaced(int UserId);
        Task<MailData> Checkout(User UserId);
    }
}
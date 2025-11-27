using Domain.Models;
using static Domain.Models.Order;

namespace Infrastructure.IRepository
{
    public interface IOrderRepo
    {
        Task<Order> AddProductToOrder(OrderDetails orderDetails);
        Task<OrderItem> UpdateOrder(OrderDetails orderDetails);
        Task<OrderItem> RemoveProductToOrder(int UserId, int ProductId);
        Task<bool> OrderPlaced(int UserId);
    }
}
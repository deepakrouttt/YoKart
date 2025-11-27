using Domain.Models;
using static Domain.Models.Order;

namespace Infrastructure.IRepository
{
    public interface ICartRepo
    {
        Task<Order> Index(int id);
        Task<HttpResponseMessage> AddProductOrder(OrderDetails orderDetails);
        Task<HttpResponseMessage> RemoveProductOrder(int id);
        Task<HttpResponseMessage> UpdateProduct(OrderDetails orderDetails);
        Task<bool> Checkout(int UserId);
    }
}
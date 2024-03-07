using Microsoft.AspNetCore.Mvc;
using YoKartApi.Models;
using static YoKartApi.Models.Order;

namespace YoKart.IServices
{
    public interface ICartServices
    {
        Task<Order> Index(int id);
        Task<HttpResponseMessage> AddProductOrder(OrderDetails orderDetails);
        Task<HttpResponseMessage> RemoveProductOrder(int id);
    }
}
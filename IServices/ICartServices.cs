using Microsoft.AspNetCore.Mvc;
using YoKartApi.Models;

namespace YoKart.IServices
{
    public interface ICartServices
    {
        Task<Order> Index(int id);
        Task<HttpResponseMessage> AddProductOrder(int id, int quantity);
        Task<HttpResponseMessage> RemoveProductOrder(int id);
    }
}
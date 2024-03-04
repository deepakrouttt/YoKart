using Microsoft.AspNetCore.Mvc;
using YoKartApi.Models;

namespace YoKart.Services
{
    public interface ICartServices
    {
        Task<Order> Index(int id);
        Task<HttpResponseMessage> AddProductOrder(int id);
    }
}
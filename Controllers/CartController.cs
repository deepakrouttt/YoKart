using Microsoft.AspNetCore.Mvc;
using YoKart.IServices;
using YoKart.Services;
using YoKartApi.Models;
using static System.Net.WebRequestMethods;

namespace YoKart.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServices _service;

        public CartController(ICartServices service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int id)
        {
            var orders =await _service.Index(id);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> AddProductOrder(int id)
        {
            var response =await _service.AddProductOrder(id);

            if (response.IsSuccessStatusCode)
            {
                return View("Index");
            }
            return View("Index","Cart");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YoKart.IServices;
using static YoKartApi.Models.Order;

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
            if (id != 0)
            {
                var orders = await _service.Index(id);
                return View(orders);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductOrder(OrderDetails orderDetails)
        {
            var response = await _service.AddProductOrder(orderDetails);
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = UserId });
            }
            return View("Index", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var response = await _service.RemoveProductOrder(id);
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = UserId });
            }
            return View("Index", "Cart");
        }

        [HttpGet]
        public async Task<String>UpdateProduct(OrderDetails orderDetails)
        {
            var response = await _service.UpdateProduct(orderDetails);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "NotFound";
        }
    }
}

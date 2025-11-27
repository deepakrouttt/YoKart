using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Domain.Models.Order;

namespace Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepo _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartRepo service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContextAccessor?.HttpContext?.Session.GetInt32("UserId"));
            if (id != 0)
            {
                var orders = await _service.Index(id);
                if (orders.OrderId == 0)
                {
                    return View();
                }
                return View(orders);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductOrder(OrderDetails orderDetails)
        {
            var response = await _service.AddProductOrder(orderDetails);
            var UserId = _httpContextAccessor?.HttpContext?.Session.GetInt32("UserId");
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
            var UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = UserId });
            }
            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public async Task<String> UpdateProduct(OrderDetails orderDetails)
        {
            var response = await _service.UpdateProduct(orderDetails);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "NotFound";
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(int id)
        {
            var Message = await _service.Checkout(id);
            if (Message == true)
            {
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

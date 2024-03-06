using Microsoft.AspNetCore.Mvc;
using YoKart.IServices;

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

        [HttpPost]
        public async Task<IActionResult> AddProductOrder(int id, int quantity)
        {
            var response = await _service.AddProductOrder(id,quantity);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = myVar.UserId });
            }
            return View("Index", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var response = await _service.RemoveProductOrder(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { id = myVar.UserId });
            }
            return View("Index", "Cart");
        }
    }
}

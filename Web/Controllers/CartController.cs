using Application.Repository;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Extensions;
using static Domain.Models.Order;

namespace Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IOrderRepo _orderRepo;

        public CartController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.GetUserId());
            if (userId != 0)
            {
                var orders = await _orderRepo.GetOrders(userId);
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
            var order = await _orderRepo.AddProductToOrder(orderDetails);

            if (order != null)
            {
                return RedirectToAction("Index");
            }
            return View("Index", "Cart");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var userId = Convert.ToInt32(User.GetUserId());
            var response = await _orderRepo.RemoveProductToOrder(userId, id);
       
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<string> UpdateOrder(OrderDetails orderDetails)
        {
            orderDetails.UserId = Convert.ToInt32(User.GetUserId());
            var response = await _orderRepo.UpdateOrder(orderDetails);
  
            return "NotFound";
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(int id)
        {
            var Message = await _orderRepo.OrderPlaced(id);
            if (Message == true)
            {
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

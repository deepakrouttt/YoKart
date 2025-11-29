using Application.Repository;
using Domain.Models;
using Infrastructure.IMailService;
using Infrastructure.IRepository;
using MailKit;
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
        private readonly IUserRepo _userRepo;
        private readonly IMailRepo _mailRepo;

        public CartController(IOrderRepo orderRepo, IUserRepo userRepo, IMailRepo mailRepo)
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _mailRepo = mailRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.GetUserId());
            if (userId != 0)
            {
                var orders = await _orderRepo.GetOrders(userId);
                if (orders == null)
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

        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var uid = Convert.ToInt32(userId);
                var orders = await _orderRepo.GetOrders(uid);
                var user = await _userRepo.GetUser(uid);

                if (orders != null)
                {
                    var mail = await _orderRepo.Checkout(user);
                    if (mail != null)
                    {
                        await _mailRepo.SendMailAsync(mail);
                    }
                    return Json(new { status = true, redirect = Url.Action("Index", "Cart") });
                }
            }
            return Json(new { status = false, redirect = Url.Action("Index", "Home") });
        }
    }
}

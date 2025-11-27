using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Domain.Models.Order;

namespace YoKart.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly YoKartDbContext _context;
        private readonly ILogger<CategoryApiController> _logger;
        private readonly IOrderRepo _service;

        public OrderApiController(YoKartDbContext context, ILogger<CategoryApiController> logger, IOrderRepo service)
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).Where(o => o.OrderItems.
            Any(oi => oi.Products.ProductId == oi.ProductId)).ToList();

            return Ok(orders);
        }

        [HttpGet("GetOrderbyUser")]
        public async Task<IActionResult> GetOrderbyUser(int id)
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Products).
                SingleOrDefault(o => o.UserId == id && o.OrderStatus == "Cart");

            return Ok(orders);
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> AddProductToOrder(OrderDetails orderDetails)
        {
            var addProductOrder = await _service.AddProductToOrder(orderDetails);
            if (addProductOrder != null)
            {
                return Ok(addProductOrder);
            }
            else
            {
                return Ok("Order list not found");
            }
        }

        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(OrderDetails orderDetails)
        {
            var addProductOrder = await _service.UpdateOrder(orderDetails);
            if (addProductOrder != null)
            {
                return Ok(addProductOrder);
            }
            else
            {
                return Ok("Order list not found");
            }
        }


        [HttpDelete("RemoveOrder")]
        public async Task<IActionResult> RemoveProductToOrder(int userid, int productid)
        {
            var RemoveOrderProduct = await _service.RemoveProductToOrder(userid, productid);
            if (RemoveOrderProduct != null)
            {
                return Ok(RemoveOrderProduct);
            }
            return NotFound();
        }

        [HttpPost("OrderPlaced")]
        public async Task<IActionResult> OrderPlaced([FromBody] int UserId)
        {
            var ifOrderPlaced = await _service.OrderPlaced(UserId);
            if (ifOrderPlaced)
            {
                return Ok("Order Placed SuccessFully");
            }
            return NotFound();
        }
    }
}

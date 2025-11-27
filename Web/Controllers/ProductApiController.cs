using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace YoKart.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly YoKartDbContext _context;
        private readonly IProductRepo _services;

        public ProductApiController(YoKartDbContext context, IProductRepo services)
        {
            _context = context;
            _services = services;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var products = _services.RandomProduct();

            return Ok(products);

        }

        [HttpGet("GetProductsRange")]
        public IActionResult GetProductsRange([FromQuery] filtering? obj)
        {
            if (obj.HighRange is 0) { obj.HighRange = Decimal.MaxValue; }
            ;
            var product = _services.Productpaging(_context.Products.ToList(), obj);
            return Ok(product);
        }

        [HttpGet("GetProductsBySearch")]
        public IActionResult GetProductsBySearch(string? search)
        {
            var product = _services.ProductSearch(_context.Products.ToList(), search);
            return Ok(product);
        }

        [HttpGet("{id}")]
        public IActionResult GetProducts(int id)
        {
            var products = _context.Products.Find(id);

            return Ok(products);
        }

    

    }
}


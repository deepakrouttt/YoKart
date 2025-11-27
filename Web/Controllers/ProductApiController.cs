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

        [HttpGet("GetProductsForSubcategory")]
        public IActionResult GetProductsForSubcategory(int subcategoryId)
        {
            var products = _context.Products.Where(p => p.SubCategoryId == subcategoryId).ToList();

            return Ok(products);
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product _product)
        {
            _context.Products.Add(_product);
            _context.SaveChanges();

            return Ok(_product);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> EditProduct(Product _product)
        {
            var product = _context.Products.FirstOrDefault(s => s.ProductId == _product.ProductId);

            if (product == null)
            {
                return NotFound("Category not found");
            }

            product.CategoryId = _product.CategoryId;
            product.SubCategoryId = _product.SubCategoryId;
            product.ProductName = _product.ProductName;
            product.ProductImage = _product.ProductImage;
            product.ProductPrice = _product.ProductPrice;
            product.ProductDescription = _product.ProductDescription;

            _context.SaveChanges();
            return Ok(product);
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(m => m.ProductId == id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }
}


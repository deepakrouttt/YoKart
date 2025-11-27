using Domain.Models;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;


        public HomeController(HttpClient client, IProductRepo productRepo, ICategoryRepo categoryRepo)
        {
            _client = client;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.RandomProduct().ConfigureAwait(false);
            if (products != null)
            {
                return View(products);
            }
            return View(null);
        }

        public async Task<IActionResult> Product(int id)
        {
            var product = await _productRepo.GetProduct(id);
            ViewData["Subcategories"] = await _categoryRepo.GetSubCategories();
            return View(product);
        }
        public IActionResult privacy()
        {
            return View();
        }

    }
}

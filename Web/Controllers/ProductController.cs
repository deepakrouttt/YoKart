using Domain.Models;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public readonly ICategoryRepo _serviceCat;
        public readonly HttpClient _client;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IProductRepo _service;
        public ProductController(ICategoryRepo serviceCat, HttpClient client, IWebHostEnvironment webHostEnvironment,
                IProductRepo proService)
        {
            _serviceCat = serviceCat;
            _client = client;
            _webHostEnvironment = webHostEnvironment;
            _service = proService;
        }
        public async Task<IActionResult> Index(filtering obj)
        {
            var products = await _service.Index(obj);
            return View("Index", products);
        }

        public async Task<IActionResult> Index_Partial(filtering obj)
        {
            var products = await _service.Index(obj);
            return PartialView("_Index", products);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var response = await _service.Create(product);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.Edit(id);
            if (product != null) { return View(product); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var response = await _service.Edit(product);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(Product product)
        {
            if (product.ProductImageFile == null) { return Redirect("Edit/" + product.ProductId); }
            var response = await _service.EditImage(product);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.Delete(id);
            if (response.IsSuccessStatusCode) { return RedirectToAction("Index"); }
            return View("Index");
        }
    }
}

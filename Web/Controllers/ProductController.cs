using Domain.Models;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public readonly ICategoryRepo _cateRepo;
        public readonly HttpClient _client;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IProductRepo _proRepo;
        public ProductController(ICategoryRepo caterepo, HttpClient client, IWebHostEnvironment webHostEnvironment,
                IProductRepo proRepo)
        {
            _cateRepo = caterepo;
            _client = client;
            _webHostEnvironment = webHostEnvironment;
            _proRepo = proRepo;
        }
        public async Task<IActionResult> Index(filtering obj)
        {
            ViewData["categories"] = await _cateRepo.GetCategories();
            ViewData["subcategories"] = await _cateRepo.GetCategories();
            var products = await _proRepo.Index(obj);
            return View("Index", products);
        }

        public async Task<IActionResult> Index_Partial(filtering obj)
        {
            ViewData["categories"] = await _cateRepo.GetCategories();
            ViewData["subcategories"] = await _cateRepo.GetCategories();
            var products = await _proRepo.Index(obj);
            return PartialView("_Index", products);
        }


        [HttpGet]
        public  JsonResult GetProductsForSubcategory(int subcategoryId)
        {
            var products = _proRepo.GetProductsForSubcategory(subcategoryId);
            return Json(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await _cateRepo.GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var response = await _proRepo.AddProduct(product);
            if (response != null) { 
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _proRepo.Edit(id);
            if (product != null) { return View(product); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var response = await _proRepo.UpdateProduct(product);
            if (response != null) {
                return RedirectToAction("Index"); 
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(Product product)
        {
            if (product.ProductImageFile == null) { return Redirect("Edit/" + product.ProductId); }
            var response = await _proRepo.EditImage(product);
            if (response != null) {
                return RedirectToAction("Index"); 
            }
            return View("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _proRepo.DeleteProduct(id);
            if (response!= null) { 
                return RedirectToAction("Index"); 
            }
            return View("Index");
        }
    }
}

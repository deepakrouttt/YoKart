using Microsoft.AspNetCore.Mvc;
using YoKart.Models;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class ProductController : Controller
    {
        public readonly ICategoryServices _serviceCat;
        public readonly HttpClient _client;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IProductSevices _service;

        public ProductController(ICategoryServices serviceCat, HttpClient client, IWebHostEnvironment webHostEnvironment,
            IProductSevices proService)
        {
            _serviceCat = serviceCat;
            _client = client;
            _webHostEnvironment = webHostEnvironment;
            _service = proService;
        }
        public async Task<IActionResult> Index(int? page, long? low, long? high)
        {
            var _cate = await _serviceCat.CategoryList();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var products = await _service.Index( low, high);
            var tempProduct = myVar.PagingProduct(products, page);
            return View("Index", tempProduct);
        }

        public async Task<IActionResult> Index_Partial(int? page, long? low, long? high)
        {
            var _cate = await _serviceCat.CategoryList();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var products = await _service.Index(low, high);
            var tempProduct = myVar.PagingProduct(products, page);
            return PartialView("_Index", tempProduct);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var _cate = await _serviceCat.CategoryList();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;
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
            var _cate = await _serviceCat.CategoryList();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;
            var product = await _service.Edit(id);
            if (product != null){ return View(product);}
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
            var product = await _service.Edit(id);

            var url = "https://localhost:44373/api/ProductApi/DeleteProduct?id=" + id;
            var response = _client.DeleteAsync(url).Result;

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImage");
            var filePath = Path.Combine(uploadsFolder, product.ProductImage);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}

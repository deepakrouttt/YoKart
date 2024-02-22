using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using YoKart.Models;
using YoKart.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace YoKart.Controllers
{
    public class ProductController : Controller
    {
        public readonly ICategoryServices _data;
        public readonly HttpClient _client;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IProductSevices _service;

        public ProductController(ICategoryServices data, HttpClient client, IWebHostEnvironment webHostEnvironment,
            IProductSevices proService)
        {
            _data = data;
            _client = client;
            _webHostEnvironment = webHostEnvironment;
            _service = proService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var _cate = await _data.CategoryData();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;
            var products = new List<Product>();
            var response = await _service.Index(page);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);
                if (data != null)
                {
                    products = data;
                }
            }
            var tempProduct = myVar.PagingProduct(products, page);
            return View("Index", tempProduct);
        }

        public async Task<IActionResult> Index_Partial(int? page, long? low, long? high)
        {
            var _cate = await _data.CategoryData();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var products = new List<Product>();
            var response = await _service.Index(page);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);
                if (data != null) { products = data; }
            }

            var rangeProduct = products.Where(m => Convert.ToInt64(m.ProductPrice) > (low ?? 0) &&
            Convert.ToInt64(m.ProductPrice) < (high ?? long.MaxValue));

            var tempProduct = myVar.PagingProduct(rangeProduct, page);

            return PartialView("_Index", tempProduct);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var data = await _data.CategoryData();

            ViewData["categories"] = data.CategoryList;
            ViewData["subcategories"] = data.SubCategoryList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (product.ProductImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\products");
                var orgFileName = Path.GetFileName(product.ProductImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, orgFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductImageFile.CopyTo(fileStream);
                }
            }

            var productUpdate = await _service.ProductSerializeImage(product);

            var url = "https://localhost:44373/api/ProductApi/AddProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var _cate = await _data.CategoryData();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var url = "https://localhost:44373/api/ProductApi/" + id;
            var response = _client.GetAsync(url).Result;
            var product = new Product();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Product>(result);

                if (data != null)
                {
                    product = data;

                    return View(product);
                }
            }
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
            var url = "https://localhost:44373/api/ProductApi/DeleteProduct?id=" + id;
            var response = _client.DeleteAsync(url).Result;

            //var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadImage");
            //var filePath = Path.Combine(uploadsFolder, imagepath);

            //if (System.IO.File.Exists(filePath))
            //{
            //    System.IO.File.Delete(filePath);
            //}

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}

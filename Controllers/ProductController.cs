using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using YoKart.Models;
using YoKart.Services;


namespace YoKart.Controllers
{
    public class ProductController : Controller
    {
        public readonly ICategoryServices _data;
        public readonly HttpClient _client;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IProductSevices _proService;

        public ProductController(ICategoryServices data, HttpClient client, IWebHostEnvironment webHostEnvironment,
            IProductSevices proService)
        {
            _data = data;
            _client = client;
            _webHostEnvironment = webHostEnvironment;
            _proService = proService;
        }

        public async Task<IActionResult> Index()
        {
            var _cate = await _data.CategoryData();
            ViewData["categories"] = _cate.CategoryList;
            ViewData["subcategories"] = _cate.SubCategoryList;

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\products");
            var imageFiles = Directory.GetFiles(uploadsFolder);

            foreach (var image in imageFiles)
            {
                GlobalVariable.imagePaths.Add(Path.GetFileName(image));
            }

            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            var response = _client.GetAsync(url).Result;
            var product = new List<Product>();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);

                if (data != null)
                {
                    product = data;

                }
                return View(product);
            }

            return View();
        }

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

            var productUpdate = await _proService.ProductSerializeImage(product);

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
            var productUpdate = await _proService.ProductSerialize(product);

            var url = "https://localhost:44373/api/ProductApi/UpdateProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(Product product)
        {
            if (product.ProductImageFile == null)
            {
                ModelState.AddModelError("ProductImageFile", "The Password field is required.");
                return Redirect("Edit/"+product.ProductId);
            }

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

            var productUpdate = await _proService.ProductSerializeImage(product);

            var url = "https://localhost:44373/api/ProductApi/UpdateProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Index");
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

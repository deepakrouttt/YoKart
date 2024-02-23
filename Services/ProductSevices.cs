using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;

namespace YoKart.Services
{
    public class ProductSevices : IProductSevices
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly HttpClient _client;
        public readonly ICategoryServices _serviceCat;

        public ProductSevices(IWebHostEnvironment webHostEnvironment, HttpClient client, ICategoryServices serviceCat)
        {
            _webHostEnvironment = webHostEnvironment;
            _client = client;
            _serviceCat = serviceCat;
        }

        public async Task<IEnumerable<Product>> Index( long? low, long? high)
        {
            var products = new List<Product>();
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\products");
            var imageFiles = Directory.GetFiles(uploadsFolder);

            foreach (var image in imageFiles)
            {
                myVar.imagePaths.Add(Path.GetFileName(image));
            }
            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Product>>(result);

                if (data != null)
                {
                    products = data;
                }
            }
            if (high == null) { high = long.MaxValue; };
            var rangeProduct = products.Where(m => Convert.ToInt64(m.ProductPrice) > (low ?? 0) &&
            Convert.ToInt64(m.ProductPrice) < high);
            return rangeProduct;
        }

        public async Task<HttpResponseMessage> Create(Product product)
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
            var productUpdate = await ProductSerializeImage(product);
            var url = "https://localhost:44373/api/ProductApi/AddProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;
            return response;
        }

        public async Task<Product> Edit(int id)
        {
            var url = "https://localhost:44373/api/ProductApi/" + id;
            var response = _client.GetAsync(url).Result;
            var product = new Product();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data ;
                }
            }
            return product;
        }

        public async Task<HttpResponseMessage> Edit(Product product)
        {
            var productUpdate = await ProductSerialize(product);

            var url = "https://localhost:44373/api/ProductApi/UpdateProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;
            return response;
        }

        public async Task<HttpResponseMessage> EditImage(Product product)
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

            var productUpdate = await ProductSerializeImage(product);

            var url = "https://localhost:44373/api/ProductApi/UpdateProduct";
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, stringContent);
            return response;
        }

        //Product Serialize
        public async Task<ProductUpdate> ProductSerialize(Product product)
        {
            var productUpdate = new ProductUpdate
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription
            };
            return productUpdate;
        }
   
        public async Task<ProductUpdate> ProductSerializeImage(Product product)
        {
            var productUpdate = new ProductUpdate
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImageFile.FileName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription
            };
            return productUpdate;
        }

    }
}

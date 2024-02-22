using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Models;

namespace YoKart.Services
{
    public class ProductSevices : IProductSevices
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly HttpClient _client;

        public ProductSevices(IWebHostEnvironment webHostEnvironment, HttpClient client)
        {
            _webHostEnvironment = webHostEnvironment;
            _client = client;
        }

        public async Task<HttpResponseMessage> Index(int? page)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\products");
            var imageFiles = Directory.GetFiles(uploadsFolder);
            foreach (var image in imageFiles) { myVar.imagePaths.Add(Path.GetFileName(image)); }

            var url = "https://localhost:44373/api/ProductApi/GetProducts";
            var response = await _client.GetAsync(url);
            return response;
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

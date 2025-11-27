using Domain.Global;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Repository
{
    public class ProductRepo : IProductRepo
    {
        public readonly IHostEnvironment _hostEnvironment;
        public readonly HttpClient _client;
        public readonly ICategoryRepo _serviceCat;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly YoKartDbContext _context;
        private static bool isAscending = true;
        private readonly string baseUrl = "https://localhost:44373/api/ProductApi/";

        public ProductRepo(IHostEnvironment hostEnvironment, HttpClient client, ICategoryRepo serviceCat, IHttpContextAccessor httpContextAccessor, YoKartDbContext context)
        {
            _hostEnvironment = hostEnvironment;
            _client = client;
            _serviceCat = serviceCat;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Product> GetProduct(int id)
        {
            var products = await _context.Products.FindAsync(id) ?? new Product();
            return products;
        }
        public async Task<IEnumerable<Product>> Index(filtering obj)
        {
            var products = new List<Product>();
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

            var imageFiles = Directory.GetFiles(uploadsFolder);

            foreach (var image in imageFiles)
            {
                YokartVar.imagePaths.Add(Path.GetFileName(image));
            }

            var url = $"{baseUrl}GetProductsRange?Page={obj.page}&LowPrice={obj.LowRange}" +
                $"&HighPrice={obj.HighRange}&Sort={obj.Sort}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductPagingData>(result);
                if (data != null)
                {
                    products = data.Product;
                    YokartVar.pageCount = data.pageCount;
                    YokartVar.pageSize = data.pageSize;
                    YokartVar.totalProduct = data.totalProduct;
                    YokartVar.currentPage = data.currentPage;
                }
            }
            return products;
        }

        public async Task<HttpResponseMessage> Create(Product product)
        {
            if (product.ProductImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

                var orgFileName = Path.GetFileName(product.ProductImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, orgFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductImageFile.CopyTo(fileStream);
                }
            }
            var productUpdate = await ProductSerializeImage(product);
            var url = $"{baseUrl}AddProduct";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;
            return response;
        }

        public async Task<Product> Edit(int id)
        {
            var url = baseUrl + id;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = _client.GetAsync(url).Result;
            var product = new Product();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data;
                }
            }
            return product;
        }

        public async Task<HttpResponseMessage> Edit(Product product)
        {
            var productUpdate = await ProductSerialize(product);

            var url = $"{baseUrl}UpdateProduct";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = _client.PutAsync(url, stringContent).Result;
            return response;
        }

        public async Task<HttpResponseMessage> EditImage(Product product)
        {
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

            var filePath = Path.Combine(uploadsFolder, product.ProductImage);

            if (System.IO.File.Exists(filePath)) { System.IO.File.Delete(filePath); }

            if (product.ProductImageFile.Length > 0)
            {
                var orgFileName = Path.GetFileName(product.ProductImageFile.FileName);
                var _filePath = Path.Combine(uploadsFolder, orgFileName);

                using (var fileStream = new FileStream(_filePath, FileMode.Create))
                {
                    product.ProductImageFile.CopyTo(fileStream);
                }
            }

            var productUpdate = await ProductSerializeImage(product);

            var url = $"{baseUrl}UpdateProduct";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(productUpdate), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, stringContent);
            return response;
        }

        public async Task<HttpResponseMessage> Delete(int id)
        {
            var product = await Edit(id);
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

            var filePath = Path.Combine(uploadsFolder, product.ProductImage);
            if (System.IO.File.Exists(filePath)) { System.IO.File.Delete(filePath); }
            var url = $"{baseUrl}DeleteProduct?id=" + id;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = _client.DeleteAsync(url).Result;
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


        public ProductPagingData Productpaging(List<Product> Products, filtering obj)
        {

            var Rangeproducts = Products.Where(m => m.ProductPrice > obj.LowRange);

            if (obj.HighRange != 0)
            {
                Rangeproducts = Rangeproducts.Where(m => m.ProductPrice < obj.HighRange);
            }

            var ProductList = Rangeproducts.ToList();
            YokartVar.pageCount = (int)Math.Ceiling(ProductList.Count / (double)YokartVar.pageSize);
            YokartVar.currentPage = obj.page ?? 1;
            var tempProduct = ProductList.Skip((YokartVar.currentPage - 1) * YokartVar.pageSize).Take(YokartVar.pageSize).ToList();
            YokartVar.totalProduct = ProductList.Count;

            if (obj.Sort != null)
            {

                switch (obj.Sort)
                {
                    case "CategoryName":
                        Rangeproducts = isAscending
                              ? tempProduct.OrderBy(m => _context.Categories.Find(m.CategoryId).CategoryName)
                              : tempProduct.OrderByDescending(m => _context.Categories.Find(m.CategoryId).CategoryName);
                        break;
                    case "SubCategoryName":
                        Rangeproducts = isAscending
                        ? tempProduct.OrderBy(m => _context.SubCategories.Find(m.SubCategoryId).SubCategoryName)
                            : tempProduct.OrderByDescending(m => _context.SubCategories.Find(m.SubCategoryId).SubCategoryName);
                        break;
                    case "ProductName":
                        Rangeproducts = isAscending
                        ? tempProduct.OrderBy(m => m.ProductName.ToLower())
                        : tempProduct.OrderByDescending(m => m.ProductName.ToLower());
                        break;
                    case "ProductImage":
                        Rangeproducts = isAscending
                        ? tempProduct.OrderBy(m => m.ProductImage)
                        : tempProduct.OrderByDescending(m => m.ProductImage);
                        break;
                    case "ProductPrice":
                        Rangeproducts = isAscending
                        ? tempProduct.OrderBy(m => m.ProductPrice)
                        : tempProduct.OrderByDescending(m => m.ProductPrice);
                        break;
                    case "ProductDescription":
                        Rangeproducts = isAscending
                        ? tempProduct.OrderBy(m => m.ProductDescription)
                        : tempProduct.OrderByDescending(m => m.ProductDescription);
                        break;
                    default:
                        break;
                }
                isAscending = !isAscending;
                return new()
                {
                    Product = Rangeproducts.ToList(),
                    pageSize = YokartVar.pageSize,
                    pageCount = YokartVar.pageCount,
                    totalProduct = YokartVar.totalProduct,
                    currentPage = YokartVar.currentPage
                };
            }

            return new()
            {
                Product = tempProduct,
                pageSize = YokartVar.pageSize,
                pageCount = YokartVar.pageCount,
                totalProduct = YokartVar.totalProduct,
                currentPage = YokartVar.currentPage
            };
        }


        public List<Product> ProductSearch(List<Product> Products, string search)
        {
            var ProductList = new List<Product>();
            if (!string.IsNullOrEmpty(search))
            {
                ProductList = Products.Where(p => (p.ProductName.ToLower().Contains(search.ToLower()))
                                                  || (p.ProductImage.ToLower().Contains(search.ToLower()))
                                                  || (_context.Categories.FirstOrDefault(m => m.CategoryId == p.CategoryId).CategoryName.ToLower().Contains(search.ToLower()))
                                                  || (_context.SubCategories.FirstOrDefault(m => m.SubCategoryId == p.SubCategoryId).SubCategoryName.ToLower().Contains(search.ToLower()))
                                                  ).ToList();
            }
            else
            {
                ProductList = Products;
            }
            return ProductList;
        }

        //Random Product Listing
        public async Task<List<Product>> RandomProduct()
        {
            var products = await _context.Products.ToListAsync();

            Random random = new Random();
            int n = products.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                var value = products[k];
                products[k] = products[n];
                products[n] = value;
            }
            return products;
        }

    }
}

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
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

            var imageFiles = Directory.GetFiles(uploadsFolder);

            foreach (var image in imageFiles)
            {
                YokartVar.imagePaths.Add(Path.GetFileName(image));
            }

            if (obj.HighRange is 0) { obj.HighRange = Decimal.MaxValue; }
            ;

            var productList = _context.Products.ToList();
            var products = Productpaging(productList, obj);
            YokartVar.pageCount = products.pageCount;
            YokartVar.pageSize = products.pageSize;
            YokartVar.totalProduct = products.totalProduct;
            
            return products.Product;
        }

        public async Task<Product> AddProduct(Product product)
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
            product.ProductName = product.ProductImageFile.FileName;
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
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

        public async Task<Product> UpdateProduct(Product product)
        {
            product.ProductName = product.ProductImageFile.FileName;
            _context.Update(product);
            _context.SaveChanges();

            return product;
        }

        public async Task<Product> EditImage(Product product)
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

            product.ProductName = product.ProductImageFile.FileName;

            _context.SaveChanges();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await Edit(id);
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "products");

            var filePath = Path.Combine(uploadsFolder, product.ProductImage);
            if (System.IO.File.Exists(filePath)) { System.IO.File.Delete(filePath); }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }

        public List<Product> GetProductsForSubcategory(int subcategoryId)
        {
            var products = _context.Products.Where(p => p.SubCategoryId == subcategoryId).ToList();

            return products;
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
            YokartVar.currentPage = obj.page;
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

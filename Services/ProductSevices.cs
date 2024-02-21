using YoKart.Models;

namespace YoKart.Services
{
    public class ProductSevices : IProductSevices
    {
        public async Task<ProductUpdate> ProductSerialize(Product obj)
        {          
            var productUpdate = new ProductUpdate
            {
                ProductId = obj.ProductId,
                CategoryId = obj.CategoryId,
                SubCategoryId = obj.SubCategoryId,
                ProductName = obj.ProductName,
                ProductImage = obj.ProductImage,
                ProductPrice = obj.ProductPrice,
                ProductDescription = obj.ProductDescription
            };
            return productUpdate;
        }
        public async Task<ProductUpdate> ProductSerializeImage(Product obj)
        {
            var productUpdate = new ProductUpdate
            {
                ProductId = obj.ProductId,
                CategoryId = obj.CategoryId,
                SubCategoryId = obj.SubCategoryId,
                ProductName = obj.ProductName,
                ProductImage = obj.ProductImageFile.FileName,
                ProductPrice = obj.ProductPrice,
                ProductDescription = obj.ProductDescription
            };
            return productUpdate;
        }
    }
}

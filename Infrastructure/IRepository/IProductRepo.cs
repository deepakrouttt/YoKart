using Domain.Models;

namespace Infrastructure.IRepository
{
    public interface IProductRepo
    {
        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> Index(filtering obj);
        Task<Product> AddProduct(Product product);
        Task<Product> Edit(int id);
        Task<Product> UpdateProduct(Product product);
        Task<Product> EditImage(Product product);
        Task<bool> DeleteProduct(int id);
        List<Product> GetProductsForSubcategory(int subcategoryId);

        //Product Serialization
        Task<ProductUpdate> ProductSerialize(Product product);
        Task<ProductUpdate> ProductSerializeImage(Product product);

        ProductPagingData Productpaging(List<Product> Products, filtering obj);

        List<Product> ProductSearch(List<Product> Products, string search);

        Task<List<Product>> RandomProduct();
    }
}
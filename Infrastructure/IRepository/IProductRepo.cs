using Domain.Models;

namespace Infrastructure.IRepository
{
    public interface IProductRepo
    {
        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> Index(filtering obj);
        Task<HttpResponseMessage> Create(Product product);
        Task<Product> Edit(int id);
        Task<HttpResponseMessage> Edit(Product product);
        Task<HttpResponseMessage> EditImage(Product product);
        Task<HttpResponseMessage> Delete(int id);

        //Product Serialization
        Task<ProductUpdate> ProductSerialize(Product product);
        Task<ProductUpdate> ProductSerializeImage(Product product);

        ProductPagingData Productpaging(List<Product> Products, filtering obj);

        List<Product> ProductSearch(List<Product> Products, string search);

        Task<List<Product>> RandomProduct();
    }
}
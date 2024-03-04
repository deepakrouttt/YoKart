using Microsoft.AspNetCore.Mvc;
using YoKart.Models;

namespace YoKart.IServices
{
    public interface IProductSevices
    {
        Task<IEnumerable<Product>> Index(filtering obj);
        Task<HttpResponseMessage> Create(Product product);
        Task<Product> Edit(int id);
        Task<HttpResponseMessage> Edit(Product product);
        Task<HttpResponseMessage> EditImage(Product product);
        Task<HttpResponseMessage> Delete(int id);

        //Product Serialization
        Task<ProductUpdate> ProductSerialize(Product product);
        Task<ProductUpdate> ProductSerializeImage(Product product);
    }
}
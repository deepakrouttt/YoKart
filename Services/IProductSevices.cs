using Microsoft.AspNetCore.Mvc;
using YoKart.Models;

namespace YoKart.Services
{
    public interface IProductSevices
    {
        Task<IEnumerable<Product>> Index( long? low, long? high);
        Task<HttpResponseMessage> Create(Product product);
        Task<Product> Edit(int id);
        Task<HttpResponseMessage> Edit(Product product);
        Task<HttpResponseMessage> EditImage(Product product);

        //Product Serialization
        Task<ProductUpdate> ProductSerialize(Product product);
        Task<ProductUpdate> ProductSerializeImage(Product product);
    }
}
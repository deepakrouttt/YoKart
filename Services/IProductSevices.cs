using Microsoft.AspNetCore.Mvc;
using YoKart.Models;

namespace YoKart.Services
{
    public interface IProductSevices
    {
        Task<HttpResponseMessage> Index(int? page);
        Task<HttpResponseMessage> Edit(Product product);
        Task<HttpResponseMessage> EditImage(Product product);

        //Product Serialization
        Task<ProductUpdate> ProductSerialize(Product product);
        Task<ProductUpdate> ProductSerializeImage(Product product);
    }
}
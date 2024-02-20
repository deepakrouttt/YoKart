using YoKart.Models;

namespace YoKart.Services
{
    public interface IProductSevices
    {
        Task<ProductUpdate> ProductSerialize(Product obj);
    }
}
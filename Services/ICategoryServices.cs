using YoKart.Models;

namespace YoKart.Services
{
    public interface ICategoryServices
    {
        Task<CategoriesView> CategoryData();
    }
}
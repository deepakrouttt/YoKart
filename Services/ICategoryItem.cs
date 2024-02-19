using YoKart.Models;

namespace YoKart.Services
{
    public interface ICategoryItem
    {
        Task<CategoriesView> CategoryData();
    }
}
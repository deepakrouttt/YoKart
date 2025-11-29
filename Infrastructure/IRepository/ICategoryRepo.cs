using Domain.Models;

namespace Infrastructure.IRepository
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetCategories();
        Task<List<SubCategory>> GetSubCategories();
        Task<bool> AddCategory(Category category);

        Task<Category> GetCategory(int id);

        Task<Category> UpdateCategory(Category category);

        Task<Category> Exist(Category category);

        Task<Category> GetSubCategory(int? id, int? page);

        Task<SubCategory> EditSub(int id);
        Task<SubCategory> EditSub(SubCategory category);
        Task<Category> RemoveCategories(int id);
        Task<SubCategory> RemoveSubCategories(int id);
    }
}
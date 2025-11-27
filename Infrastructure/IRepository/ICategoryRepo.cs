using Domain.Models;

namespace Infrastructure.IRepository
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetCategories();
        Task<List<SubCategory>> GetSubCategories();
        Task<HttpResponseMessage> Create(Category category);

        Task<Category> Edit(int id);

        Task<HttpResponseMessage> Edit(Category category);

        Task<HttpResponseMessage> Exist(Category category);

        Task<Category> IndexSub(int? id, int? page);

        Task<SubCategory> EditSub(int id);

        Task<HttpResponseMessage> EditSub(SubCategory category);
    }
}
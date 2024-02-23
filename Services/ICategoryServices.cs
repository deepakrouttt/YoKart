using Microsoft.AspNetCore.Mvc;
using YoKart.Models;

namespace YoKart.Services
{
    public interface ICategoryServices
    {   //category All Data
        Task<CategoriesView> CategoryList();

        Task<HttpResponseMessage> Create(Category category);

        Task<Category> Edit(int id);

        Task<HttpResponseMessage> Edit(Category category);

        Task<HttpResponseMessage> Exist(Category category);

        Task<Category> IndexSub(int? id, int? page);

        Task<SubCategory> EditSub(int id);

        Task<HttpResponseMessage> EditSub(SubCategory category);
    }
}
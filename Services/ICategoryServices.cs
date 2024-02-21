using Microsoft.AspNetCore.Mvc;
using YoKart.Models;

namespace YoKart.Services
{
    public interface ICategoryServices
    {
        Task<CategoriesView> CategoryData();
        Task<HttpResponseMessage> Create(Category category);
        Task<HttpResponseMessage> Exist(Category category);
    }
}
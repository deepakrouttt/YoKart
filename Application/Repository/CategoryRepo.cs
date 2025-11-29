using Domain.Global;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        HttpClient _client = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly YoKartDbContext _context;

        public CategoryRepo(HttpClient client, IHttpContextAccessor httpContextAccessor, YoKartDbContext context)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories.Include(c => c.SubCategories).ToListAsync();
            return categories;
        }

        public async Task<List<SubCategory>> GetSubCategories()
        {
            var subcategories = await _context.SubCategories.ToListAsync();
            return subcategories;
        }
        public async Task<bool> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return true;
        }
        public async Task<Category> Exist(Category category)
        {

            var existingCategory = _context.Categories.Find(category.CategoryId);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.CategoryName = category.CategoryName;
            existingCategory.SubCategories = category.SubCategories;

            _context.SaveChanges();

            return category;
        }

        public async Task<Category> GetCategory(int id)
        {
            var categories = _context.Categories.Include(c => c.SubCategories).ToList();
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var existingCategory = _context.Categories.Find(category.CategoryId);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.CategoryName = category.CategoryName;

            _context.SaveChanges();
            return existingCategory;
        }

        public async Task<Category> GetSubCategory(int? id, int? page)
        {

            var data = await GetCategory(id ?? 1);
            if (data != null)
            {
                var category = Service.PagingSubCategory(data, page);
                return category;
            }
            return new Category();
        }

        public async Task<SubCategory> EditSub(int id)
        {

            var subcategory = _context.SubCategories.FirstOrDefault(c => c.SubCategoryId == id);
            return subcategory;
        }

        public async Task<SubCategory> EditSub(SubCategory category)
        {
            var existingCategory = _context.SubCategories.FirstOrDefault(c => c.SubCategoryId == category.SubCategoryId);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.SubCategoryName = category.SubCategoryName;

            _context.SaveChanges();
            return existingCategory;
        }
        public async Task<Category> RemoveCategories(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<SubCategory> RemoveSubCategories(int id)
        {
            var Subcategory = await _context.SubCategories.FindAsync(id);

            _context.SubCategories.Remove(Subcategory);
            await _context.SaveChangesAsync();

            return Subcategory;
        }
    }
}
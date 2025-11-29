using Domain.Global;
using Domain.Models;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public HttpClient _client = new HttpClient();
        public readonly ICategoryRepo _cateRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryController(HttpClient client, ICategoryRepo cateRepo, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;   
            _cateRepo = cateRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var categories = await _cateRepo.GetCategories();
            var tempCategory = Service.PagingCategory(categories, page);
            return View("Index", tempCategory);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _cateRepo.GetCategories();
            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories(int id)
        {
            var categories = await _cateRepo.GetCategory(id);
            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> _Index(int? page)
        {
            var categories = await _cateRepo.GetCategories();
            var tempCategory = Service.PagingCategory(categories, page);
            return PartialView("_Index", tempCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var response = await _cateRepo.AddCategory(category);
            if (response) { 
                return RedirectToAction("Index");
            }
            return View("Create", category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _cateRepo.GetCategory(id);
            if (category != null) { return View(category); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var response = await _cateRepo.UpdateCategory(category);
            if (response != null) {
                return RedirectToAction("Index");
            }
            return View("Edit", category);
        }

        [HttpPost]
        public async Task<IActionResult> Exist(Category category)
        {
            var response = await _cateRepo.Exist(category);
            if (response != null) { 
                return RedirectToAction("Index");
            }
            return View("Edit", category);
        }

        public async Task<IActionResult> IndexSub(int? id, int? page)
        {
            var subCategory = await _cateRepo.GetSubCategory(id, page);
            if (subCategory != null) { return View("IndexSub", subCategory); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexSub_Partial(int? id, int? page)
        {
            var subCategory = await _cateRepo.GetSubCategory(id, page);
            return PartialView("_IndexSub", subCategory);
        }
        [HttpGet]
        public async Task<IActionResult> EditSub(int id)
        {
            var subcategory = await _cateRepo.EditSub(id);
            if (subcategory != null) { return View(subcategory); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditSub(SubCategory category)
        {
            var response = await _cateRepo.EditSub(category);
            if (response != null)
            {
                return Redirect("~/Category/IndexSub/" + category.CategoryId);
            }
            return View("EditSsub", category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _cateRepo.RemoveCategories(id);
            if (response != null)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSub(int id)
        {
            var response = await _cateRepo.RemoveSubCategories(id);
            if (response != null)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }

    }
}

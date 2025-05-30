using Budget_MVC.Data;
using Budget_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            CategoryViewModel vm = new CategoryViewModel();
            vm.Categories = categories;
            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var existingCategory = _dbContext.Categories
                .FirstOrDefault(c => c.Name.ToLower() == categoryViewModel.Category.Name.ToLower());

            if (existingCategory != null)
            {
                return Json(new { success = false, message = "Category already exists." });
            }

            categoryViewModel.Category.CreatedDate = DateTime.Now;
            categoryViewModel.Category.UpdatedDate = DateTime.Now;

            _dbContext.Categories.Add(categoryViewModel.Category);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Category added successfully" });

        }
        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }
            return Json(new { success = true, data = category });

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            var category = _dbContext.Categories.Find(categoryViewModel.Category.Id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }

            category.Name = categoryViewModel.Category.Name;
            category.Description = categoryViewModel.Category.Description;
            category.UpdatedDate = DateTime.Now;
            var existingCategory = _dbContext.Categories
                .FirstOrDefault(c => c.Name.ToLower() == categoryViewModel.Category.Name.ToLower() && c.Id != categoryViewModel.Category.Id);
            if (existingCategory != null)
            {
                category.Name = existingCategory.Name;
                return Json(new { success = false, message = "Category with this name already exists." });
            }
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Category updated successfully." });
        }

        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Category deleted successfully." });
            }
            return Json(new { success = false, message = "Category not found." });
        }
    }
}

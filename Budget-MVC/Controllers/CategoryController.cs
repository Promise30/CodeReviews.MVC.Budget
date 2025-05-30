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
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(categoryViewModel.Category);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Category added successfully" });
            }
            // Return validation errors
            return Json(new { success = false, message = "Validation failed", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        }
        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category != null)
            {
                return Json(new { success = true, data = category });
            }
            return Json(new { success = false, message = "Category not found." });
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = _dbContext.Categories.Find(categoryViewModel.Category.Id);
                if (category != null)
                {
                    category.Name = categoryViewModel.Category.Name;
                    category.Description = categoryViewModel.Category.Description;
                    _dbContext.SaveChanges();
                    return Json(new { success = true, message = "Category updated successfully." });
                }
                return Json(new { success = false, message = "Category not found." });
            }
            // Return validation errors or not found message
            return Json(new { success = false, message = "Validation failed.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
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

using Budget_MVC.Data;
using Budget_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using X.PagedList.Extensions;

namespace Budget_MVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ApplicationDbContext _context;

        public TransactionController(ILogger<TransactionController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        
        }
        public IActionResult Index(int? page, string? searchTerm, FilterParameters? filterParameters)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var query = _context.Transactions.Include(x=> x.Category).AsQueryable();
            var categories = _context.Categories.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => EF.Functions.Like(t.Name , $"%{searchTerm}%"));
            }
            if (filterParameters != null)
            {
                if (!string.IsNullOrEmpty(filterParameters.Category))
                {
                    query = query.Where(t => t.Category != null && t.Category.Name.Equals(filterParameters.Category));
                }
                if (filterParameters.Date.HasValue)
                {
                    query = query.Where(t => t.TransactionDate.Date == filterParameters.Date.Value.Date);
                }
            }

            // Pagination logic
            var paginatedTransaction = query.OrderByDescending(x=> x.TransactionDate).ToPagedList(pageNumber, pageSize);

            TransactionViewModel vm = new TransactionViewModel
            {
                Transactions = paginatedTransaction,
                Categories = categories,
                Transaction = new Transaction() 
            };
            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(TransactionViewModel transactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                _logger.LogInformation(JsonSerializer.Serialize(fieldErrors));
                return Json(new { success = false, message = "Validation failed", fieldErrors });
            }
            transactionViewModel.Transaction.CreatedDate = DateTime.Now;
            transactionViewModel.Transaction.UpdatedDate = DateTime.Now;
            _context.Transactions.Add(transactionViewModel.Transaction);
            _context.SaveChanges();

            return Json(new { success = true, message = "Transaction created successfully." });
        }
        [HttpGet]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t=> t.Id == id);
            if (transaction != null)
            {
                return Json(new { success = true, data = transaction });
            }
            return Json(new { success = false, message = "Transaction not found." });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(TransactionViewModel transactionViewModel)
        {
            if (ModelState.IsValid)
            {
                var transaction = _context.Transactions.Find(transactionViewModel.Transaction.Id);
                if (transaction != null)
                {
                    transaction.Name = transactionViewModel.Transaction.Name;
                    transaction.Description = transactionViewModel.Transaction.Description;
                    transaction.Amount = transactionViewModel.Transaction.Amount;
                    transaction.CategoryId = transactionViewModel.Transaction.CategoryId;
                    transaction.TransactionDate = transactionViewModel.Transaction.TransactionDate;
                    transaction.UpdatedDate = DateTime.Now;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Transaction updated successfully." });
                }
                return Json(new { success = false, message = "Transaction not found." });
            }
            var fieldErrors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, message = "Validation failed", fieldErrors });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Transaction deleted successfully." });
            }
            return Json(new { success = false, message = "Transaction not found." });
        }
    }
}

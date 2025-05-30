using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Budget_MVC.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        [Display(Name = "Transaction Date")]    
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}



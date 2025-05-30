using X.PagedList;

namespace Budget_MVC.Models
{
    public class TransactionViewModel
    {
        public IPagedList<Transaction> Transactions { get; set; }  = new PagedList<Transaction>(new List<Transaction>(), 1, 5);
        public List<Category> Categories  { get; set; } = new List<Category>();
        public Transaction Transaction { get; set; }
    }
}

namespace Budget_MVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Category()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}

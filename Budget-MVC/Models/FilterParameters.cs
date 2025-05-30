namespace Budget_MVC.Models
{
    public class FilterParameters
    {
        public string? Category { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

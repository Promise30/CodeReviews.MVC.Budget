using Budget_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Transaction>().Property(c => c.Amount).HasPrecision(18, 2);

        }
    }
}

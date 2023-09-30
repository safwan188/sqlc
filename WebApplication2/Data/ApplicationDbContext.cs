using Microsoft.EntityFrameworkCore;
using WebApplication2.Models; // Ensure this is the correct namespace for your Address model.
using WebApplication2;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        // Other DbSet properties for other tables
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Banker> Bankers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account>? Account { get; set; }
        public DbSet<WebApplication2.LedgerEntry>? LedgerEntry { get; set; }
        public DbSet<WebApplication2.Loan>? Loan { get; set; }
        public DbSet<WebApplication2.Manager>? Manager { get; set; }
        public DbSet<WebApplication2.Investment>? Investment { get; set; }
        public DbSet<WebApplication2.InvestmentAccount>? InvestmentAccount { get; set; }

    }
}

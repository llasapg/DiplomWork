using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiplomaSolution.Models
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<FormFile> CustomerFiles { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}

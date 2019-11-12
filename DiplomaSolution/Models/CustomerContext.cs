using System;
using Microsoft.EntityFrameworkCore;

namespace DiplomaSolution.Models
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

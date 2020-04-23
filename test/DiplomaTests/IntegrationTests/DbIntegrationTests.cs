using DiplomaSolution.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DiplomaTests.IntegrationTests
{
    public class DbIntegrationTests
    {
        /// <summary>
        /// Will be improved in case, when i will have more free time
        /// </summary>
        [Fact]
        public async void CheckSomeDbFeatureTest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            var options = new DbContextOptionsBuilder<CustomerContext>().UseSqlite(connection).Options;

            using(var context = new CustomerContext(options))
            {
                context.Database.EnsureCreated();

                context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole() { Id = "1", Name = "user", NormalizedName= "USER"});

                context.SaveChanges();
            }

            using (var context = new CustomerContext(options))
            {
                var allRoles = await context.Roles.ToListAsync();

                Assert.Equal("1", allRoles[0].Id);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using net_core_backend.Models;


namespace net_core_backend.Context
{
    public class ContextFactoryTesting : IDesignTimeDbContextFactory<IndividualProjectContext>, IContextFactory
    {
        private readonly string connectionString;

        public ContextFactoryTesting(string connectionString)
        {
            this.connectionString = connectionString;
        }

#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public IndividualProjectContext CreateDbContext(string[] args = null)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            var options = new DbContextOptionsBuilder<IndividualProjectContext>();
            options.UseInMemoryDatabase("TestingDatabase");

            return new IndividualProjectContext(options.Options);
        }
    }
}

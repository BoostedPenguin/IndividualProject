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

        public IndividualProjectContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<IndividualProjectContext>();
            options.UseSqlServer(connectionString);

            return new IndividualProjectContext(options.Options);
        }
    }
}

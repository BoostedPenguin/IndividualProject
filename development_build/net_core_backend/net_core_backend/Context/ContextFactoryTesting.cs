using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using net_core_backend.Models;


namespace net_core_backend.Context
{
    public class ContextFactoryTesting : IDesignTimeDbContextFactory<IndividualProjectContext>, IContextFactory
    {
        public IndividualProjectContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<IndividualProjectContext>();
            options.UseSqlServer("Data Source=.;Initial Catalog=TestingDatabase;Integrated Security=True;");

            return new IndividualProjectContext(options.Options);
        }
    }
}

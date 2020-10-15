using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<IndividualProjectContext>, IContextFactory
    {
        private readonly string connectionString;
        public ContextFactory(string connectionString)
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

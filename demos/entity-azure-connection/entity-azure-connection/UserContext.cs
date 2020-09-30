using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace entity_azure_connection
{
    public class UserContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
          : base(options)
        {

        }
    }
}

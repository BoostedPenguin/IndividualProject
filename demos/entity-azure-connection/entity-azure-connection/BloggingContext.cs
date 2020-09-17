using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace entity_azure_connection
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public BloggingContext(DbContextOptions<BloggingContext> options)
          : base(options)
        {

        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        
        [Required]
        public string Urls { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}

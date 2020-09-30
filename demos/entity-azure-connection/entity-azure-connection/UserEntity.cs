using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace entity_azure_connection
{
    public class UserEntity
    {
        [Key]
        public string User_ID { get; set; }
        public string Image_url { get; set; }
        public string Name { get; set; }
        public string Country{ get; set; }
        public string City{ get; set; }
        public bool Suggestions { get; set; }
        public bool Notifications { get; set; }
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public DateTime Created_at { get; set; } = DateTime.Now;
    }
}

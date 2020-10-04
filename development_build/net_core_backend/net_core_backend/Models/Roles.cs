using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class Roles : DefaultModel
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}

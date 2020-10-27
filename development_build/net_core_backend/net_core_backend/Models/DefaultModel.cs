using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public enum Role
    {
        User,
        Admin
    }

    public enum Transportation
    {
        Walk,
        Car,
        Bus
    }

    public class DefaultModel
    {
        public int Id { get; set; }
    }
}

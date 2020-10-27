using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class WishList : DefaultModel
    {
        public WishList()
        {
            WishListLocations = new HashSet<WishListLocations>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public Transportation Transportation { get; set; }
        public int? Distance { get; set; }
        public int? Duration { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<WishListLocations> WishListLocations { get; set; }
    }
}

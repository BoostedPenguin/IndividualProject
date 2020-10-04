using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class WishListLocations : DefaultModel
    {
        public int? WishlistId { get; set; }
        public string Name { get; set; }
        public double? Long { get; set; }
        public double? Lang { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual WishList Wishlist { get; set; }
    }
}

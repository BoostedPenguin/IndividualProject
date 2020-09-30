using System;
using System.Collections.Generic;

namespace entity_azure_connection.Models
{
    public partial class WishListLocations
    {
        public int LocationId { get; set; }
        public int? WishlistId { get; set; }
        public string Name { get; set; }
        public double? Long { get; set; }
        public double? Lang { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual WishList Wishlist { get; set; }
    }
}

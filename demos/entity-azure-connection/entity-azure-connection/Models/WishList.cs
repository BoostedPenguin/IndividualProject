using System;
using System.Collections.Generic;

namespace entity_azure_connection.Models
{
    public partial class WishList
    {
        public WishList()
        {
            WishListLocations = new HashSet<WishListLocations>();
        }

        public int WishlistId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int? TransportationId { get; set; }
        public int? Distance { get; set; }
        public int? Duration { get; set; }

        public virtual Transportation Transportation { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<WishListLocations> WishListLocations { get; set; }
    }
}

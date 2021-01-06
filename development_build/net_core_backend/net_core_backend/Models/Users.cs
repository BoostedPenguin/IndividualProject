using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class Users : DefaultModel
    {
        public Users()
        {
            SupportTicket = new HashSet<SupportTicket>();
            UserKeywords = new HashSet<UserKeywords>();
            UserTrips = new HashSet<UserTrips>();
            WishList = new HashSet<WishList>();
        }

        public string Auth { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool? Suggestions { get; set; }
        public bool? Notifications { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<SupportTicket> SupportTicket { get; set; }
        public virtual ICollection<UserKeywords> UserKeywords { get; set; }
        public virtual ICollection<UserTrips> UserTrips { get; set; }
        public virtual ICollection<WishList> WishList { get; set; }
    }
}

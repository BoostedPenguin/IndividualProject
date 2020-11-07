using net_core_backend.ViewModel;
using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class UsersViewModel
    {
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool? Suggestions { get; set; }
        public bool? Notifications { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<SupportTicketViewModel> SupportTicket { get; set; }
        public virtual ICollection<UserKeywordsViewModel> UserKeywords { get; set; }
        public virtual ICollection<UserTripsViewModel> UserTrips { get; set; }
        public virtual ICollection<WishListViewModel> WishList { get; set; }
    }
}

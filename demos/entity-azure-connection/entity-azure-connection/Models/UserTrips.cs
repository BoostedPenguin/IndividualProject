using System;
using System.Collections.Generic;

namespace entity_azure_connection.Models
{
    public partial class UserTrips
    {
        public UserTrips()
        {
            UserTripLocations = new HashSet<UserTripLocations>();
        }

        public int TripId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int TransporationId { get; set; }
        public int? Distance { get; set; }
        public double? Duration { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Transportation Transporation { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<UserTripLocations> UserTripLocations { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class UserTripLocations : DefaultModel
    {
        public int TripId { get; set; }
        public string Name { get; set; }
        public double Lang { get; set; }
        public double Long { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual UserTrips Trip { get; set; }
    }
}

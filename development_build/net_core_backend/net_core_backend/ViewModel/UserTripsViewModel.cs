using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.ViewModel
{
    public class UserTripsViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Transportation { get; set; }
        public int? Distance { get; set; }
        public double? Duration { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<LocationsViewModel> Locations { get; set; }
    }
}

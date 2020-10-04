﻿using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class Transportation
    {
        public Transportation()
        {
            UserTrips = new HashSet<UserTrips>();
            WishList = new HashSet<WishList>();
        }

        public int TransportId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserTrips> UserTrips { get; set; }
        public virtual ICollection<WishList> WishList { get; set; }
    }
}

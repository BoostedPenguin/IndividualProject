using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GoogleDataObject
    {
        public GoogleDataObject()
        {
            Types = new List<string>();
            WeekdayText = new List<string>();
        }

        // Geocoding locations
        public string Country { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CityAlt { get; set; }
        public string PlaceId { get; set; }

        // Distance matrix
        public string Duration { get; set; }
        public string Distance { get; set; }



        // Place details
        public string Business_status { get; set; }
        public string International_phone_number { get; set; }
        public string Name { get; set; }
        public string OpenNow { get; set; }
        public List<string> WeekdayText { get; set; }
        public double? Rating { get; set; }
        public string PhotoReference { get; set; }
        public string Website { get; set; }
        public int? User_ratings_total { get; set; }
        public string Vicinity { get; set; }


        // Coordinates
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Location types
        public List<string> Types { get; set; }
        public string MainType { get; set; }

        // Misc
        public bool AlreadyInWishlist { get; set; } = false;
    }
}

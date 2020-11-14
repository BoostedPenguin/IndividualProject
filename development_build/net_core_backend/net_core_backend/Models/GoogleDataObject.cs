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
        }

        // Geocoding locations
        public string Country { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CityAlt { get; set; }

        // Distance matrix
        public string Duration { get; set; }
        public string Distance { get; set; }

        // Coordinates
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Location types
        public List<string> Types { get; set; }
    }
}

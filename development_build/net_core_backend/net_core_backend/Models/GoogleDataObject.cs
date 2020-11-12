using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public struct GoogleDataObject
    {
        public string Duration { get; set; }
        public string Distance { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class KeywordAddress : DefaultModel
    {
        public int KeywordId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CityAlt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual UserKeywords Keyword { get; set; }
    }
}

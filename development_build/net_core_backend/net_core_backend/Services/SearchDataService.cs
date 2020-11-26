using Microsoft.AspNetCore.Http;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class SearchDataService : ISearchDataService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IGoogleService googleService;

        public SearchDataService(IHttpContextAccessor httpContext, IGoogleService googleService)
        {
            this.httpContext = httpContext;
            this.googleService = googleService;
        }

        public async Task<GoogleDataObject> SearchForLocation(string location, string type = null)
        {
            var result = await googleService.LocationFromLandmark(location);

            if (result == null) throw new ArgumentException("There wasn't a match for this search location");

            return result;
        }

    }
}

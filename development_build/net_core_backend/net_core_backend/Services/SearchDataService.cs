using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static net_core_backend.Services.GoogleDataService;

namespace net_core_backend.Services
{
    public class SearchDataService : DataService<UserKeywords>, ISearchDataService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IGoogleService googleService;
        private readonly IContextFactory contextFactory;
        private readonly ISuggestionService suggestionService;

        public SearchDataService(IHttpContextAccessor httpContext, IGoogleService googleService, IContextFactory contextFactory, ISuggestionService suggestionService) : base(contextFactory)
        {
            this.httpContext = httpContext;
            this.googleService = googleService;
            this.contextFactory = contextFactory;
            this.suggestionService = suggestionService;
        }

        public async Task<GooglePlaceObject[]> GetSuggestions()
        {
                return await suggestionService.Main();
        }

        public async Task<GoogleDataObject> GetPlaceByID(string placeId)
        {
            var result = await googleService.GetLocationFromPlaceID(placeId);
            
            if (result == null) throw new ArgumentException("There wasn't a match for this search location");

            return result;
        }

        public async Task<GoogleDataObject> SearchForLocation(string location, string type = null)
        {
            var result = await googleService.LocationFromLandmark(location);

            if (result == null) throw new ArgumentException("There wasn't a match for this search location");

            try
            {
                httpContext.GetCurrentAuth();
                await AddKeyword(location, result);
            }
            catch (Exception)
            {
                // Person is a guest
                // Person isn't a guest, but the keyword already exists in the database
            }

            return result;
        }

        public async Task<GooglePlaceObject[]> GetGuestSuggestions(UserKeywords keywords)
        {
            var result = await googleService.GetNearbyPlaces(keywords);

            if(result.Count > 8) 
                return result.Take(8).ToArray();

            return result.ToArray();
        }

        private async Task AddKeyword(string keyword, GoogleDataObject result)
        {
            if (result == null) throw new ArgumentException("Data object was empty");
            if (keyword == null) throw new ArgumentException("Keyword was empty");
            var userId = await base.GetUserId(httpContext.GetCurrentAuth());
            if (userId == 0) throw new Exception("There isn't such a user in the system");

            using (var a = contextFactory.CreateDbContext())
            {
                if (a.UserKeywords.FirstOrDefaultAsync(x => x.Keyword == keyword) != null)
                    throw new ArgumentException("This keyword already exists in our database");

                var address = new KeywordAddress()
                {
                    City = result.City,
                    CityAlt = result.CityAlt,
                    Country = result.Country,
                    CountryCode = result.CountryCode,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude
                };

                var types = new List<KeywordType>();

                foreach (var r in result.Types)
                {
                    types.Add(new KeywordType() { Type = r });
                }

                var key = new UserKeywords() { UserId = userId, Keyword = keyword, KeywordAddress = address, KeywordType = types };

                await a.UserKeywords.AddAsync(key);
                await a.SaveChangesAsync();
            }
        }

    }
}

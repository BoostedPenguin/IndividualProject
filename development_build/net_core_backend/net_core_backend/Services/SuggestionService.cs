using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class SuggestionService : DataService<UserKeywords>
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;

        public SuggestionService(IContextFactory contextFactory, IHttpContextAccessor httpContext) :  base(contextFactory)
        {
            this.contextFactory = contextFactory;
            this.httpContext = httpContext;
        }


        public class DataStatistics
        {
            public DataStatistics()
            {
                CountryCount = new Dictionary<string, int>();
                CityCount = new Dictionary<string, int>();
                TypeCount = new Dictionary<string, int>();
            }
            public Dictionary<string, int> CountryCount { get; set; }
            public Dictionary<string, int> CityCount { get; set; }
            public Dictionary<string, int> TypeCount { get; set; }
        }



        public async Task Main(string type = null)
        {
            var keywords = await Fetch(type);

            var stats = GenerateStatistics(keywords);
        }

        /// <summary>
        /// Fetches all keywords for the specific user
        /// </summary>
        public async Task<List<UserKeywords>> Fetch(string type)
        {
            using(var a = contextFactory.CreateDbContext())
            {
                // Fetch
                var userId = await base.GetUserId(httpContext.GetCurrentAuth());
                var keywords = await a.UserKeywords.Include(x => x.KeywordAddress).Include(x => x.KeywordType).Where(x => x.UserId == userId).ToListAsync();

                // Filter for type
                keywords = type != null ? keywords = keywords.Where(x => x.KeywordType.Any(x => x.Type == type)).ToList() : keywords;

                return keywords;
            }
        }

        /// <summary>
        /// Generate statistics from keywords
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public DataStatistics GenerateStatistics (List<UserKeywords> keywords)
        {
            // Store counts
            var stat = new DataStatistics();

            foreach (var k in keywords)
            {
                foreach (var t in k.KeywordType)
                {
                    if (stat.TypeCount.ContainsKey(t.Type)) stat.TypeCount[t.Type] += 1;
                    else stat.TypeCount.Add(t.Type, 0);
                }

                foreach (var ad in k.KeywordAddress)
                {
                    if (ad.Country != null)
                    {
                        if (stat.CountryCount.ContainsKey(ad.Country)) stat.CountryCount[ad.Country] += 1;
                        else stat.CountryCount.Add(ad.Country, 0);
                    }

                    if (ad.City != null)
                    {
                        if (stat.CityCount.ContainsKey(ad.City)) stat.CityCount[ad.City] += 1;
                        else stat.CityCount.Add(ad.City, 0);
                    }

                    if (ad.City == null && ad.CityAlt != null)
                    {
                        if (stat.CityCount.ContainsKey(ad.CityAlt)) stat.CityCount[ad.CityAlt] += 1;
                        else stat.CityCount.Add(ad.CityAlt, 0);
                    }
                }
            }

            stat.CountryCount = stat.CountryCount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            stat.CityCount = stat.CityCount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            stat.TypeCount = stat.TypeCount.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);


            return stat;
        }

        public void GetPreferences(DataStatistics data)
        {
            // Creates a not so-linear chance for a location
            Random r = new Random();
            int randomNumber = r.Next(0, data.CountryCount.Values.Sum());

            int accumulated = 0;
            foreach(var a in data.CountryCount)
            {
                accumulated += a.Value;
                if (randomNumber <= accumulated)
                {
                    //This item

                    break;
                }
            }

            accumulated = 0;
            randomNumber = r.Next(0, data.TypeCount.Values.Count);

            foreach(var a in data.TypeCount)
            {
                accumulated += a.Value;
                if (randomNumber <= accumulated)
                {
                    // This item

                    break;
                }
            }


            randomNumber = r.Next(0, 2);
            if(randomNumber == 0)
            {
                // Give country suggestion
            }
            else
            {
                // Give type suggestion
            }
            
        }

        public void Rank()
        {

        }
    }
}

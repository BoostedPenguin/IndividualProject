using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using net_core_backend.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static net_core_backend.Services.GoogleDataService;

namespace net_core_backend.Services
{
    public class SuggestionService : DataService<UserKeywords>, ISuggestionService
    {
        private int MaxTimesToDisplay = 5;
        private int RequestedSuggestions = 10;
        private int TotalCitiesSuggestions = 4;

        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly IGoogleService googleService;
        private readonly Random r;
        private int userId;

        public SuggestionService(IContextFactory contextFactory, IHttpContextAccessor httpContext, IGoogleService googleService) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
            this.httpContext = httpContext;
            this.googleService = googleService;
            r = new Random();
        }


        /// <summary>
        /// Main algorithm function. Call to receive suggestions.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<GooglePlaceObject[]> Main(string type = null)
        {
            userId = await base.GetUserId(httpContext.GetCurrentAuth());

            if (userId == 0) throw new ArgumentException("User is invalid");

            var data = await GenerateExistingSuggestions();


            //Testing
            if(data != null)
            {
                var c = data.GroupBy(x => x.TimesDisplayed).ToList();
            }


            if (data == null)
            {
                await GenerateNewSuggestions(type);

                data = await GenerateExistingSuggestions();

                if (data.Count == 0) throw new ArgumentException("Not enough suggestions generated");

                return await GetSuggestions(data);
            }

            return await GetSuggestions(data);
        }


        /// <summary>
        /// Checks if current file suggestions aren't exchausted and uses them if possible
        /// </summary>
        /// <returns></returns>
        private async Task<List<GooglePlaceObject>> GenerateExistingSuggestions()
        {
            var read = await ReadSuggestionsFile(userId);

            if (read == null) return null;

            if (read.Count >= RequestedSuggestions)
            {
                return read;
            }

            return null;
        }


        /// <summary>
        /// Generates new suggestions through Google API if current ones are exchausted
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task GenerateNewSuggestions(string type)
        {
            var keywords = await Fetch(type);

            if (keywords.Count == 0)
                throw new ArgumentException("Not enough keywords to create a suggestion list");

            // This will give only 1, so the suggestions will be only from 1 city
            List<GooglePlaceObject> total = new List<GooglePlaceObject>();


            // Generate for cities
            for(int i = 0; i < TotalCitiesSuggestions; i++)
            {
                if (keywords.Count == 0) break;

                var main = GetRandomKeyword(keywords);

                // Exclude this city from future requests
                keywords = keywords.Where(x => x.KeywordAddress.City != main.KeywordAddress.City).ToList();

                total.AddRange(await googleService.GetNearbyPlaces(main, type));
            }

            await WriteSuggestionsFile(total, userId);
        }


        /// <summary>
        /// Gets 10 random suggestions from the available and adjusts their usage
        /// </summary>
        /// <param name="suggestions"></param>
        /// <returns></returns>
        private async Task<GooglePlaceObject[]> GetSuggestions(List<GooglePlaceObject> suggestions)
        {
            suggestions = suggestions.Where(x => x.TimesDisplayed < MaxTimesToDisplay).ToList();

            List<GooglePlaceObject> temp = new List<GooglePlaceObject>();
            temp.AddRange(suggestions);

            List<GooglePlaceObject> result = new List<GooglePlaceObject>();


            for (int i = 0; i < RequestedSuggestions; i++)
            {
                int current = r.Next(0, temp.Count);

                var selected = temp[current];

                result.Add(selected);

                temp.Remove(selected);



                var z = suggestions.IndexOf(suggestions.FirstOrDefault(y => y == selected));
                suggestions[z].TimesDisplayed += 1;


                if (suggestions[z].TimesDisplayed >= MaxTimesToDisplay)
                {
                    //temp.Remove(suggestions[z]);
                    suggestions.Remove(suggestions[z]);
                }
            }

            // Testing
            var c = suggestions.GroupBy(x => x.TimesDisplayed).ToList();


            await WriteSuggestionsFile(suggestions, userId);
            return result.ToArray();
        }

        /// <summary>
        /// Fetches all keywords for the specific user
        /// </summary>
        private async Task<List<UserKeywords>> Fetch(string type)
        {
            using (var a = contextFactory.CreateDbContext())
            {
                //userId = userId == 0 ? await base.GetUserId(httpContext.GetCurrentAuth()) : userId;

                var keywords = await a.UserKeywords.Include(x => x.KeywordAddress).Include(x => x.KeywordType).Where(x => x.UserId == userId).ToListAsync();

                // Filter for type
                keywords = type != null ? keywords = keywords.Where(x => x.KeywordType.Any(x => x.Type == type)).ToList() : keywords;

                return keywords;
            }
        }


        /// <summary>
        /// Gets a random keyword based on percentage from the given
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        private UserKeywords GetRandomKeyword(List<UserKeywords> keywords)
        {
            var countriesCount = keywords.GroupBy(x => x.KeywordAddress.Country)
            .Select(x => new
            {
                Count = x.Count(),
                x.First().KeywordAddress.Country,
            }).OrderByDescending(x => x.Count).ToDictionary(x => x.Country, y => y.Count);

            if (countriesCount.Count == 0)
                throw new ArgumentException("Not enough countries to get a suggestion from!");


            // Creates a not so-linear chance for a location

            string country = null;
            string city = null;

            int randomNumber = r.Next(0, countriesCount.Values.Sum());

            int accumulated = 0;


            foreach (var a in countriesCount)
            {
                accumulated += a.Value;
                if (randomNumber <= accumulated)
                {
                    //This country
                    country = a.Key;
                    break;
                }
            }

            keywords = keywords.Where(x => x.KeywordAddress.Country == country).ToList();

            var citiesCount = keywords.GroupBy(x => x.KeywordAddress.City)
            .Select(x => new
            {
                Count = x.Count(),
                x.First().KeywordAddress.City,
            }).OrderByDescending(x => x.Count).ToDictionary(x => x.City, y => y.Count);

            if (citiesCount.Count == 0)
                throw new ArgumentException("Not enough cities to generate a suggestion");

            accumulated = 0;

            randomNumber = r.Next(0, citiesCount.Values.Sum());

            foreach (var a in citiesCount)
            {
                accumulated += a.Value;
                if (randomNumber <= accumulated)
                {
                    //This city
                    city = a.Key;
                    break;
                }
            }


            // Doesn't matter which exactly as long as it has relative address
            var word = keywords.Where(x => x.KeywordAddress.City == city && x.KeywordAddress.Country == country).First();

            return word;
        }


        /// <summary>
        /// Gets root project path
        /// </summary>
        /// <returns></returns>
        private string GetSuggestionFilesPath()
        {
            string baseDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            string directory = $"{baseDirectory}SuggestionFiles\\";

            if (!Directory.Exists(directory))
            {
                _ = Directory.CreateDirectory(directory);
            }

            return directory;
        }

        /// <summary>
        /// Writes suggestions json file for specific user
        /// </summary>
        /// <param name="places"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task WriteSuggestionsFile(List<GooglePlaceObject> places, int userId)
        {
            string jsonObjects = JsonConvert.SerializeObject(places);

            var path = GetSuggestionFilesPath();

            string fileName = $"{path}user_id_{userId}.json";

            await File.WriteAllTextAsync(fileName, jsonObjects.ToString());
        }


        /// <summary>
        /// Reads suggestions json file for specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<GooglePlaceObject>> ReadSuggestionsFile(int userId)
        {
            var path = GetSuggestionFilesPath();

            string fileName = $"{path}user_id_{userId}.json";

            if(File.Exists(fileName))
            {
                var jsonData = await File.ReadAllTextAsync(fileName);
                return JsonConvert.DeserializeObject<List<GooglePlaceObject>>(jsonData);
            }

            return null;
        }
    }
}

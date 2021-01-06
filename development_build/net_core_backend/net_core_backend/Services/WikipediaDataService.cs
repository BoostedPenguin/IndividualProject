using Microsoft.AspNetCore.Http;
using net_core_backend.Context;
using net_core_backend.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class WikipediaDataService : IWikipediaDataService
    {
        /// <summary>
        /// This is a testing idea, wikipedia extracts should be handled from front-end!
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        public WikipediaDataService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<string> GetWikipediaPage(string place)
        {
            place = "Ягодинска пещера";
            place = place.Replace(" ", "%20");

            // Search wiki for correct page
            string response = await GetStringAsync($"https://en.wikipedia.org/w/api.php?format=json&action=opensearch&search={place}&limit=1&namespace=0");
            dynamic res = JsonConvert.DeserializeObject(response);
            string title = null;
            try
            {
                title = res[1][0];

            }
            catch (Exception)
            {
                return "There wasn't a description available for this location";
            }


            // Extract from wiki the summary page
            string responseBody = await GetStringAsync($"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=1&explaintext&redirects=1&titles={title}");
            dynamic result = JsonConvert.DeserializeObject(responseBody);

            var data = new WikipediaDataObject();

            foreach (var a in result.query.pages)
            {
                foreach(var b in a)
                {
                    data.PageID = b.pageid;
                    data.Title = b.title;
                    data.Summary = b.extract;

                    return data.Summary;
                }
            }
            return null;
        }

        public class WikipediaDataObject
        {
            public int PageID { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
        }

        private async Task<string> GetStringAsync(string url)
        {
            var client = clientFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}

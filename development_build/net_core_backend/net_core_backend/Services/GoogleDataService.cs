using net_core_backend.Models;
using net_core_backend.Services.Extensions;
using net_core_backend.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class GoogleDataService : IGoogleService
    {
        public static string GoogleKey;
        private readonly IHttpClientFactory clientFactory;

        public GoogleDataService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<double[]> CoordinatesFromLocation(string location)
        {
            if (location == null) throw new ArgumentException("Empty location");

            string responseBody = await GetStringAsync($"https://maps.google.com/maps/api/geocode/json?address={location}");
            
            dynamic stuff = JsonConvert.DeserializeObject(responseBody);

            double lat = stuff.results[0].geometry.location.lat;
            double lng = stuff.results[0].geometry.location.lng;

            return new double[] { lat, lng };
        }


        public async Task<string[]> DistanceDurationBetweenLocations(string location1, string location2, Transportation transportation)
        {
            if (location1 == null || location2 == null) throw new ArgumentException("You need to fill in a valid location!");
            
            var city1 = await CoordinatesFromLocation(location1);
            var city2 = await CoordinatesFromLocation(location2);

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={city1[0]},{city1[1]}&destinations={city2[0]},{city2[1]}&mode={transportation.SetTransportation()}");

            dynamic stuff = JsonConvert.DeserializeObject(responseBody);
            string distance = stuff.rows[0].elements[0].distance.text;
            string duration = stuff.rows[0].elements[0].duration.text;

            return new string[2] { distance, duration };
        }

        public async Task<string[]> DistanceDurationBetweenLocations(double[] location1, double[] location2, Transportation transportation)
        {
            if (location1 == null || location2 == null) throw new ArgumentException("You need to fill in a valid location!");

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={location1[0]},{location1[1]}&destinations={location2[0]},{location2[1]}&mode={transportation.SetTransportation()}");

            dynamic stuff = JsonConvert.DeserializeObject(responseBody);
            string distance = stuff.rows[0].elements[0].distance.text;
            string duration = stuff.rows[0].elements[0].duration.text;

            return new string[2] { distance, duration };
        }



        private async Task<string> GetStringAsync(string url)
        {
            var client = clientFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(url + $"&key={GoogleKey}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}

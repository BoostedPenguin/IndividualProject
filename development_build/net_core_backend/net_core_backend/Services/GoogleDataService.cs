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

        public async Task<GoogleDataObject> CoordinatesFromLocation(string location)
        {
            if (location == null) throw new ArgumentException("Empty location");

            string responseBody = await GetStringAsync($"https://maps.google.com/maps/api/geocode/json?address={location}");
            
            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");


            double lat = result.results[0].geometry.location.lat;
            double lng = result.results[0].geometry.location.lng;

            return new GoogleDataObject() { Latitude = lat, Longitude = lng };
        }


        public async Task<GoogleDataObject> LocationFromLandmark(string landmark, string[] givenType = null)
        {
            if (landmark == null) throw new ArgumentException("Empty string");

            //Creates a types string
            string outputType = "&types=";
            outputType = givenType != null ? outputType + string.Join("|", givenType) : "";

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={landmark}{outputType}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            var data = new GoogleDataObject();

            foreach(var res in result.results[0].address_components)
            {
                if (data.City == null && res.types[0] == "locality")
                {
                    data.City = res.long_name;
                }
                else if (data.City == null && data.CityAlt == null && res.types[0] == "administrative_area_level_1")
                {
                    data.CityAlt = res.long_name;
                }
                else if (data.Country == null && res.types[0] == "country")
                {
                    data.Country = res.long_name;
                    data.CountryCode = res.short_name;
                }

                if (data.City != null && data.Country != null)
                {
                    break;
                }
            }

            foreach (var type in result.results[0].types)
            {
                data.Types.Add((string)type);
            }

            data.Latitude = result.results[0].geometry.location.lat;
            data.Longitude = result.results[0].geometry.location.lng;
            data.PlaceId = result.results[0].place_id;

            return data;
        }

        public async Task<GoogleDataObject> LocationFromCoordinates(GoogleDataObject coordinates)
        {
            if (coordinates.Latitude == null || coordinates.Longitude == null) throw new ArgumentException("Empty coordinates");

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/geocode/json?latlng={coordinates.Latitude},{coordinates.Longitude}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            var data = new GoogleDataObject();

            foreach (var res in result.results)
            {
                if(data.City == null && res.types[0] == "locality")
                {
                    foreach(var address in res.address_components)
                    {
                        if(address.types[0] == "locality")
                        {
                            data.City = address.long_name;
                            break;
                        }
                    }
                }
                else if(data.City == null && data.CityAlt == null && res.types[0] == "administrative_area_level_1")
                {
                    foreach(var alt in res.address_components)
                    {
                        if (alt.types[0] == "administrative_area_level_1")
                        {
                            data.CityAlt = alt.long_name;
                            break;
                        }
                    }
                }
                else if(data.Country == null && res.types[0] == "country")
                {
                    data.Country = res.address_components[0].long_name;
                    data.CountryCode = res.address_components[0].short_name;
                }

                if(data.City != null && data.Country != null)
                {
                    break;
                }
            }

            foreach (var type in result.results[0].types)
            {
                data.Types.Add((string)type);
            }

            data.Latitude = result.results[0].geometry.location.lat;
            data.Longitude = result.results[0].geometry.location.lng;

            return data;
        }


        public async Task<GoogleDataObject> DistanceDurationBetweenLocations(string location1, string location2, Transportation transportation)
        {
            if (location1 == null || location2 == null) throw new ArgumentException("You need to fill in a valid location!");
            
            var city1 = await CoordinatesFromLocation(location1);
            var city2 = await CoordinatesFromLocation(location2);

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={city1.Latitude},{city1.Longitude}&destinations={city2.Latitude},{city2.Longitude}&mode={transportation.SetTransportation()}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");


            string distance = result.rows[0].elements[0].distance.text;
            string duration = result.rows[0].elements[0].duration.text;

            return new GoogleDataObject() { Distance = distance, Duration = duration };
        }


        public async Task<GoogleDataObject> DistanceDurationBetweenLocations(GoogleDataObject latLngLocation1, GoogleDataObject latLngLocation2, Transportation transportation)
        {
            if (latLngLocation1.Latitude == null || latLngLocation1.Longitude == null || latLngLocation2.Latitude == null || latLngLocation2.Longitude == null) 
                throw new ArgumentException("You need to fill in a valid location!");

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?origins={latLngLocation1.Latitude},{latLngLocation1.Longitude}&destinations={latLngLocation2.Latitude},{latLngLocation2.Longitude}&mode={transportation.SetTransportation()}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            string distance = result.rows[0].elements[0].distance.text;
            string duration = result.rows[0].elements[0].duration.text;

            return new GoogleDataObject() { Distance = distance, Duration = duration };
        }


        public async Task<List<GooglePlaceObject>> GetNearbyPlaces(UserKeywords input, string type = null, int radius = 10000)
        {
            //location lng ltd
            // Dont include radius if rankby DISTANCE exists
            // keyword - name type address etc.
            // rankny prominence
            // type - ONLY ONE TYPE MAY BE SUPPLIED


            if (input.Keyword == null) throw new ArgumentException("Keyword name is missing");

            GoogleDataObject coordinates;
            if (input.KeywordAddress.Latitude == null || input.KeywordAddress.Longitude == null)
            {
                coordinates = await CoordinatesFromLocation(input.Keyword);
            }
            else
            {
                coordinates = new GoogleDataObject
                {
                    Latitude = input.KeywordAddress.Latitude,
                    Longitude = input.KeywordAddress.Longitude
                };
            }


            type = type != null ? $"&type={type}" : "";

            //string keyword = input.Keyword != null ? $"&keyword={input.Keyword.Replace(" ", "+")}" : "";

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={coordinates.Latitude},{coordinates.Longitude}&rankby=prominence&radius={radius}{type}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            var output = new List<GooglePlaceObject>();

            foreach (var a in result.results)
            {
                output.Add(new GooglePlaceObject()
                {
                    Guid = Guid.NewGuid(),
                    BusinessStatus = a.business_status,
                    Name = a.name,
                    PlaceId = a.place_id,
                    Vicinity = a.vicinity,
                    Latitude = a.geometry.location.lat,
                    Longtitude = a.geometry.location.lng,
                    Rating = a.rating,
                    PhotosWidth = a.photos[0].width,
                    PhotosHeight = a.photos[0].height,
                    PhotoReference = a.photos[0].photo_reference,
                });
            }

            return output;
        }

        public class GooglePlaceObject
        {
            public Guid Guid { get; set; }
            public int TimesDisplayed { get; set; }
            public string BusinessStatus { get; set; } //Optional
            public string Name { get; set; }
            public string PlaceId { get; set; }
            public string Vicinity { get; set; }
            public double? Latitude { get; set; }
            public double? Longtitude { get; set; }

            public double? Rating { get; set; } //Optional

            public int? PhotosHeight { get; set; } //Optional
            public int? PhotosWidth { get; set; } //Optional
            public string PhotoReference { get; set; } //Optional
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

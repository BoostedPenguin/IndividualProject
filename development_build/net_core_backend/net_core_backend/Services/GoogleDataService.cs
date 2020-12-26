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

            string fields = $"business_status,international_phone_number,name,opening_hours,rating,website,user_ratings_total,vicinity,photos";

            string responsePlace = await GetStringAsync($"https://maps.googleapis.com/maps/api/place/details/json?place_id={result.results[0].place_id}&fields={fields}");
            dynamic resultPlace = JsonConvert.DeserializeObject(responsePlace);

            if (resultPlace.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");


            var data = new GoogleDataObject();

            // Get location details

            GetLocationFromResponse(result.results[0], ref data);

            // Google place details
            GetDetailsFromResponse(resultPlace.result, ref data);


            return data;
        }

        public async Task<GoogleDataObject> GetLocationFromPlaceID(string placeId)
        {
            string fields = $"business_status,international_phone_number,name,opening_hours,rating,website,user_ratings_total,vicinity,photos,address_component,adr_address,geometry,vicinity,type,place_id";
            
            string responsePlace = await GetStringAsync($"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&fields={fields}");
            dynamic resultPlace = JsonConvert.DeserializeObject(responsePlace);
            
            if (resultPlace.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            var data = new GoogleDataObject();

            //
            GetDetailsFromResponse(resultPlace.result, ref data);

            //
            GetLocationFromResponse(resultPlace.result, ref data);

            return data;

        }

        private void GetLocationFromResponse(dynamic result, ref GoogleDataObject data)
        {

            foreach (var res in result.address_components)
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

            //foreach (var type in result.results[0].types)
            //{
            //    data.Types.Add((string)type);
            //}

            data.Latitude = result.geometry.location.lat;
            data.Longitude = result.geometry.location.lng;
            data.PlaceId = result.place_id;
        }

        private void GetDetailsFromResponse(dynamic resultPlace, ref GoogleDataObject data)
        {
            data.Business_status = resultPlace.business_status;
            data.International_phone_number = resultPlace.international_phone_number;
            data.Name = resultPlace.name;
            if (resultPlace.photos != null)
            {
                data.PhotoReference = resultPlace.photos[0].photo_reference;
            }
            if (resultPlace.opening_hours != null)
            {
                data.OpenNow = resultPlace.opening_hours.open_now;

                foreach (var a in resultPlace.opening_hours.weekday_text)
                {
                    data.WeekdayText.Add(a.Value);
                }
            }
            data.Website = resultPlace.website;
            data.User_ratings_total = resultPlace.user_ratings_total;
            data.Vicinity = resultPlace.vicinity;
            data.Rating = resultPlace.rating;
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

        public async Task DistanceBetweenMultipleLocations(string origin, string[] destination)
        {
            string destinationString = string.Join("|place_id:", destination);
            //string destinationString = destination[0];

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=place_id:{origin}&destinations=place_id:{destinationString}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");
        }

        public async Task<GoogleDirectionsObject[]> DirectionsServiceTest(string origin, string destination, string[] locations)
        {
            var waypointsString = string.Join("|place_id:", locations);

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&waypoints=optimize:true|place_id:{waypointsString}");
            
            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK") throw new ArgumentException("An unexpected error occured while contacting google API");

            var dataObjects = new List<GoogleDirectionsObject>();
            foreach(var a in result.routes[0].legs)
            {
                dataObjects.Add(new GoogleDirectionsObject()
                {
                    DistanceString = a.distance.text,
                    Distance = a.distance.value,
                    Duration = a.duration.value,
                    DurationString = a.duration.text,
                    End_Address = a.end_address,
                    End_Location_Lat = a.end_location.lat,
                    End_Location_Lng = a.end_location.lng,
                    Start_Address = a.start_address,
                    Start_Location_Lat = a.start_location.lat,
                    Start_Location_Lng = a.start_location.lng,
                });
            }

            return dataObjects.ToArray();
        }

        public class GoogleDirectionsObject
        {
            public string DistanceString { get; set; }
            public int Distance { get; set; }
            public string DurationString { get; set; }
            public int Duration { get; set; }
            public string End_Address{ get; set; }
            public double End_Location_Lat{ get; set; }
            public double End_Location_Lng { get; set; }
            public string Start_Address { get; set; }
            public double Start_Location_Lat { get; set; }
            public double Start_Location_Lng { get; set; }
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

        public async Task<List<GooglePlaceObject>> GetNearbyPlaces(UserKeywords input, string type = null, int radius = 25000, string searchKeyword = "attraction")
        {
            //location lng ltd
            // Dont include radius if rankby DISTANCE exists
            // keyword - name type address etc.
            // rankny prominence
            // type - ONLY ONE TYPE MAY BE SUPPLIED

            //if (input.Keyword == null) throw new ArgumentException("Keyword name is missing");

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
            searchKeyword = searchKeyword != null ? $"&keyword={searchKeyword}" : "";

            string responseBody = await GetStringAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={coordinates.Latitude},{coordinates.Longitude}&rankby=prominence&radius={radius}{type}{searchKeyword}");

            dynamic result = JsonConvert.DeserializeObject(responseBody);

            if (result.status != "OK" && result.status != "ZERO_RESULTS") throw new ArgumentException("An unexpected error occured while contacting google API");

            var output = new List<GooglePlaceObject>();

            foreach (var a in result.results)
            {
                if(a.photos != null)
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
                        PhotoReference = a.photos[0].photo_reference,
                    });
                }
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

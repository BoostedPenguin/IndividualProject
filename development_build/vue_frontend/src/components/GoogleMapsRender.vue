<template>
  <div id="map"></div>
</template>

<script>
import axios from "axios";
import { getInstance } from "../auth/authWrapper";

export default {
  data() {
    return {};
  },
  created() {
    this.InitiateAuth();
    //this.GenerateMap();
  },
  methods: {
    InitiateAuth() {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          this.GetWaypoints();
        }
      });

      if (instance.loading == false) {
        this.GetWaypoints();
      }
    },
    GenerateMap(waypoints) {
      const directionsService = new window.google.maps.DirectionsService();

      const map = new window.google.maps.Map(document.getElementById("map"), {
        zoom: 7,
        center: { lat: 41.85, lng: -87.65 },
        streetViewControl: false,
        rotateControl: false,
      });
      const directionsRenderer = new window.google.maps.DirectionsRenderer({
        map: map,
      });

      this.calculateAndDisplayRoute(
        directionsService,
        directionsRenderer,
        waypoints
      );
    },
    calculateAndDisplayRoute(directionsService, directionsRenderer, waypoints) {
      console.log(waypoints);
      class waypointObject {
        constructor(googleBs) {
          this.location = googleBs;
        }
      }

      let waypointsFinal = [];
      waypoints.forEach((element) => {
        waypointsFinal.push(
          new waypointObject(
            new window.google.maps.LatLng(element.latitude, element.longitude)
          )
        );
      });

      directionsService.route(
        {
          origin: "Eindhoven",
          destination: "Sofia",
          waypoints: waypointsFinal,
          optimizeWaypoints: true,
          travelMode: "DRIVING",
        },
        (response, status) => {
          if (status === "OK") {
            console.log("Did it return positive?");
            directionsRenderer.setDirections(response);
            //this.showSteps(response, markerArray, map);
          } else {
            window.alert("Directions request failed due to " + status);
          }
        }
      );
    },
    showSteps(directionResult, markerArray, map) {
      // For each step, place a marker, and add the text to the marker's infowindow.
      // Also attach the marker to an array so we can keep track of it and remove it
      // when calculating new routes.
      const myRoute = directionResult.routes[0].legs[0];
      for (let i = 0; i < myRoute.steps.length; i++) {
        const marker = (markerArray[i] =
          markerArray[i] || new window.google.maps.Marker());
        marker.setMap(map);
        marker.setPosition(myRoute.steps[i].start_location);
      }
    },
    async GetWaypoints() {
      if (!this.$auth.isAuthenticated) return;
      let authToken = await this.$auth.getTokenSilently();
      axios
        .get(`${this.$store.state.base_url}/wishlist/locations`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          //Content
          this.GenerateMap(data.data);
          console.log(data.data);
        })
        .catch((err) => console.log(err));
    },
  },
};
</script>

<style>
#map {
  height: 400px;
}
</style>
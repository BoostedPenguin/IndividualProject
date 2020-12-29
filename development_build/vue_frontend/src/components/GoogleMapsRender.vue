<template>
  <div>
    <div v-show="isReady" id="map"></div>
    <div v-show="!isReady">
      <div class="text-center mt-5">
        <p class="">Generating map...</p>
        <div class="spinner-border text-primary" role="status" />
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { getInstance } from "../auth/authWrapper";

export default {
  data() {
    return {
      isReady: false,
    };
  },
  props: ["tripId"],
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
        mapTypeControl: false,
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
      class waypointObject {
        constructor(googleBs) {
          this.location = googleBs;
        }
      }

      let origin = null;
      let destination = null;
      let waypointsFinal = [];
      waypoints.locations.forEach((element) => {
        if (element.origin_Destination == "ORIGIN") {
          origin = new window.google.maps.LatLng(element.lang, element.long);
          return;
        } else if (element.origin_Destination == "DESTINATION") {
          destination = new window.google.maps.LatLng(
            element.lang,
            element.long
          );
          return;
        }
        waypointsFinal.push(
          new waypointObject(
            new window.google.maps.LatLng(element.lang, element.long)
          )
        );
      });
      console.log(waypoints.transportation);
      directionsService.route(
        {
          origin: origin,
          destination: destination,
          waypoints: waypointsFinal,
          optimizeWaypoints: true,
          travelMode: "DRIVING",
        },
        (response, status) => {
          if (status === "OK") {
            let reconstructed = [];
            let order = response.routes[0].waypoint_order;

            reconstructed.push(
              waypoints.locations.filter(
                (e) => e.origin_Destination == "ORIGIN"
              )[0]
            );

            let temp = waypoints.locations.filter(
              (e) => e.origin_Destination == null
            );
            order.forEach((e) => {
              reconstructed.push(temp[e]);
            });

            reconstructed[0].distance = 0;
            reconstructed[0].distance_text = "Start";

            reconstructed[0].duration = 0;
            reconstructed[0].duration_text = "Start";

            reconstructed.push(
              waypoints.locations.filter(
                (e) => e.origin_Destination == "DESTINATION"
              )[0]
            );

            for (let count = 1; count < reconstructed.length; count++) {
              reconstructed[count].distance =
                response.routes[0].legs[count - 1].distance.value;
              reconstructed[count].distance_text =
                response.routes[0].legs[count - 1].distance.text;

              reconstructed[count].duration =
                response.routes[0].legs[count - 1].duration.value;
              reconstructed[count].duration_text =
                response.routes[0].legs[count - 1].duration.text;
            }

            waypoints.locations = reconstructed;
            console.log(response);
            console.log(reconstructed);
            this.$store.commit("SET_SelectedTrip", waypoints);

            directionsRenderer.setDirections(response);
          } else {
            window.alert("Directions request failed due to " + status);
          }
          this.isReady = true;
        }
      );
    },
    async GetWaypoints() {
      if (!this.$auth.isAuthenticated) return;
      let authToken = await this.$auth.getTokenSilently();

      axios
        .get(`${this.$store.state.base_url}/trip/${this.tripId}`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.GenerateMap(data.data);
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
<template>
  <div class="main-menu">
    <div class="container">
      <div v-if="loadingTrips" class="text-center pt-5">
        <p class="">Fetching trips...</p>
        <div class="spinner-border text-primary" role="status" />
      </div>
      <div v-else>
        <div
          v-if="getTrips != null && getTrips.length > 0"
          class="table-responsive pt-5"
        >
          <table class="table table-bordered">
            <thead>
              <tr>
                <th scope="col">ID</th>
                <th scope="col">Trip name</th>
                <th scope="col">Created</th>
                <th scope="col">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="trip in getTrips" v-bind:key="trip.id">
                <th>{{ trip.id }}</th>
                <td>{{ trip.name }}</td>
                <td>
                  {{ DisplayDate(trip) }}
                </td>
                <td>
                  <router-link
                    :to="{
                      name: 'TripOverview',
                      params: { tripId: trip.id },
                    }"
                    class="text-decoration-none"
                  >
                    <button class="btn btn-primary btn-block">View</button>
                  </router-link>
                </td>
                <td>
                  <button class="btn btn-danger" @click="RemoveTrip(trip.id)">
                    Remove Trip
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else>
          <p>You haven't created any support tickets</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getInstance } from "../auth/authWrapper";

import axios from "axios";
import { mapState } from "vuex";
export default {
  data() {
    return {
      loadingTrips: false,
    };
  },
  computed: mapState({
    getTrips: (state) => state.userTrips,
  }),
  mounted() {
    this.InitiateAuth();
  },
  methods: {
    InitiateAuth() {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          this.GetUserTrips();
        }
      });

      if (instance.loading == false) {
        this.GetUserTrips();
      }
    },
    async GetUserTrips() {
      const authToken = await this.$auth.getTokenSilently();

      this.loadingTrips = true;
      await axios
        .get(`${this.$store.state.base_url}/trip`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_UserTrips", data.data);
        })
        .catch((error) => {
          this.error = error.response.data;
        });
      this.loadingTrips = false;
    },

    async RemoveTrip(id) {
      const authToken = await this.$auth.getTokenSilently();

      this.loadingTrips = true;

      await axios
        .delete(`${this.$store.state.base_url}/trip/${id}`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_UserTrips", data.data);
        })
        .catch((error) => {
          this.error = error.response.data;
        });
      this.loadingTrips = false;
    },
    DisplayDate(tick) {
      let date;
      if (tick.updatedAt == null) {
        date = tick.createdAt;
      } else if (tick.updatedAt >= tick.createdAt) {
        date = tick.updatedAt;
      }
      let final = new Date(Date.parse(date));
      return final.toLocaleString();
    },
  },
};
</script>

<style>
.view-button {
  background-color: var(--penguin-secondary-variant);
  color: white;
}
.view-button:hover {
  background-color: var(--penguin-secondary);
}
.create-button {
  background-color: var(--penguin-primary);
  border-radius: 1rem;
}
.create-button:hover {
  background-color: var(--penguin-primary-variant);
}

.spinner-text {
  font-size: 2em;
  color: white;
}

.main-content {
  min-height: 600px;
  display: flex;
  background-color: rgb(255, 255, 255);
}
</style>
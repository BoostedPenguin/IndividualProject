<template>
  <div class="main-menu">
    <transition name="basic-fade" v-if="!set_transport_page">
      <div class="container">
        <h1 class="py-5 text-center">
          Select an origin and destination for your trip!
        </h1>

        <!-- Alert error -->
        <transition name="basic-fade">
          <div class="alert alert-error" v-show="error" role="alert">
            {{ error }}
          </div>
        </transition>

        <table class="table table-responsive-sm">
          <thead>
            <tr>
              <th scope="col">Name</th>
              <th scope="col">LatLong</th>
              <th scope="col">Origin</th>
              <th scope="col">Destination</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="loc in getWishlistLocations" :key="loc.id">
              <td>{{ loc.name }}</td>
              <td>{{ `${loc.long}, ${loc.lang}` }}</td>
              <td>
                <input
                  type="radio"
                  name="origin"
                  :checked="loc.origin_Destination == 'ORIGIN'"
                  @change="SendOriginDestination(loc, loc.id, 'ORIGIN')"
                />
              </td>
              <td>
                <input
                  type="radio"
                  name="destination"
                  :checked="loc.origin_Destination == 'DESTINATION'"
                  @change="SendOriginDestination(loc, loc.id, 'DESTINATION')"
                />
              </td>
            </tr>
          </tbody>
        </table>
        <button class="btn btn-primary btn-block" @click="ProceedToTransport()">
          Proceed
        </button>
      </div>
    </transition>

    <transition name="basic-fade" v-else-if="set_transport_page">
      <wishlist-select-transport
        :set_transport_page.sync="set_transport_page"
      />
    </transition>
  </div>
</template>

<script>
import { mapState } from "vuex";
import axios from "axios";
import WishlistSelectTransport from "./WishlistSelectTransport.vue";
export default {
  components: {
    WishlistSelectTransport,
  },
  data() {
    return {
      //data
      error: "",
      set_transport_page: false,

      origin: true,
      destination: false,
    };
  },
  computed: mapState({
    getWishlistLocations: (state) => state.wishlist.locations,
  }),
  created() {},
  methods: {
    async ValidateUser() {
      let authToken = "";
      try {
        authToken = await this.$auth.getTokenSilently();
      } catch (err) {
        console.log("Person ain't logged");
        this.error = "You aren't logged in!";
        this.$router.push({ name: "Home" });
        return;
      }
      return authToken;
    },
    async SendOriginDestination(item, locid, origindestination) {
      let authToken = await this.ValidateUser();

      axios
        .patch(
          `${this.$store.state.base_url}/wishlist/locations/status`,
          {
            locationId: locid,
            od: origindestination,
          },
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then(() => {
          item.origin_Destination = origindestination;
        })
        .catch((err) => {
          console.log(err);
          this.error = err.response.data;
        });
    },
    async ProceedToTransport() {
      let authToken = await this.ValidateUser();

      axios
        .get(`${this.$store.state.base_url}/wishlist/locations/status`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          if (data.data) {
            this.set_transport_page = true;
          }
        })
        .catch((err) => {
          console.log(err.response.data);
          this.error = err.response.data;
        });
    },
  },
};
</script>

<style>
.main-menu {
  min-height: 600px;
  background-color: #ffffff;
}
</style>
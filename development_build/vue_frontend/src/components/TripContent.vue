<template>
  <div class="main-menu">
    <div class="container pt-5">
      <h1 class="mb-5 text-center">Trip overview</h1>
    </div>
    <div class="container">
      <div class="row">
        <div class="col-lg-6 col-12">
          <table class="table table-bordered table-responsive-sm">
            <thead>
              <tr>
                <th scope="col" colspan="2">Locations</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="trip in currentTrip.locations" :key="trip.id">
                <td>
                  <b-link
                    :to="{
                      name: 'SearchItemView',
                      params: { placeId: trip.placeId },
                    }"
                    class="text-decoration-none"
                  >
                    {{ trip.name }}
                  </b-link>
                </td>
                <td>
                  <small>
                    {{
                      trip.origin_Destination
                        ? trip.origin_Destination.toLowerCase()
                        : ""
                    }}
                  </small>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="col-lg-6 col-12">
          <google-maps-render :tripId="tripId" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import GoogleMapsRender from "./GoogleMapsRender.vue";
import { getInstance } from "../auth/authWrapper";
import { mapState } from "vuex";

export default {
  components: {
    GoogleMapsRender,
  },
  data() {
    return {};
  },

  computed: mapState({
    currentTrip: (state) => state.trip,
  }),
  props: ["tripId"],
  created() {},
  methods: {
    InitiateAuth() {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          this.GetTrip();
        }
      });

      if (instance.loading == false) {
        this.GetTrip();
      }
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
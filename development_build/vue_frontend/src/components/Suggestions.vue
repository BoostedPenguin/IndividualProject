<template>
  <div class="main-suggestions">
    <div class="container-fluid">
      <div
        class="d-flex flex-column align-items-center justify-content-center"
        v-if="loadingSuggestions"
      >
        <div class="row">
          <img
            src="../assets/penguin.gif"
            class="img-fluid"
            width="150px"
            alt=""
          />
        </div>
        <div class="row">
          <p class="spinner-text text-center">Generating Suggestions...</p>
        </div>
      </div>
      <div class="text-center mt-5" v-else-if="error">
        <!-- Alert error -->
        <transition name="basic-fade">
          <div class="alert alert-error" role="alert">
            {{ error }}
          </div>
        </transition>
      </div>
      <div class="row mt-5" v-else>
        <div
          class="col-xl-3 col-lg-4 col-md-4 col-sm-12 col-12 pt-3"
          v-for="l in getSug"
          v-bind:key="l.guid"
        >
          <SuggestionObject :placeLocation="l" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getInstance } from "../auth/authWrapper";
import SuggestionObject from "./SuggestionObject";

import axios from "axios";
import { mapState } from "vuex";

export default {
  name: "SuggestionsGenerator",

  components: {
    SuggestionObject,
  },

  data() {
    return {
      loadingSuggestions: true,
      error: "",
    };
  },

  created() {
    this.InitiateAuth(this.GetSuggestions);
  },

  computed: mapState({
    getSug: (state) => state.suggestions.data,
  }),

  methods: {
    InitiateAuth(fn) {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          fn(instance);
        }
      });

      if (instance.loading == false) {
        this.GetSuggestions(instance);
      }
    },

    async GetGuestSuggestions(data) {
      axios
        .get(
          `${this.$store.state.base_url}/search/suggestions/guest/${data.latitude}/${data.longitude}`
        )
        .then((data) => {
          this.$store.commit("SET_Suggestions", data);
          this.loadingSuggestions = false;
        })
        .catch((err) => {
          this.loadingSuggestions = false;

          this.error = err.response.data;
        });
    },

    async GetSuggestions(instance) {
      // Guest / Not logged in
      if (!this.$auth.isAuthenticated) {
        console.log("HELLOOOO");
        axios
          .get(
            `http://api.ipstack.com/check?access_key=${this.$store.state.ip_stack}`
          )
          .then((data) => {
            this.GetGuestSuggestions(data.data);
          })
          .catch((err) => {
            this.error = err.response.data;
          });
      }
      await instance.getTokenSilently().then((authToken) => {
        // do authorized API calls with auth0 authToken here
        console.log(authToken);
        axios
          .get(`${this.$store.state.base_url}/search/suggestions`, {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          })
          .then((data) => {
            this.loadingSuggestions = false;
            this.$store.commit("SET_Suggestions", data);
          })
          .catch((err) => {
            this.loadingSuggestions = false;
            this.error = err.response.data;
          });
      });
    },
  },
};
</script>

<style>
body {
  background-color: var(--penguin-primary-variant);
}

.loading-spinner {
  width: 5em;
  height: 5em;
  color: var(--penguin-secondary-variant);
}

.spinner-text {
  font-size: 2em;
  color: white;
}

/* Card body control  */
.gallery-cover {
  width: 100%;
  min-height: 100px;
  height: 20vw;
  object-fit: cover;
}

@media (max-width: 768px) {
  .gallery-cover {
    height: 60vw;
  }
}
.container-fluid {
  padding-right: 50px;
  padding-left: 50px;
  margin-right: auto;
  margin-left: auto;
}
</style>
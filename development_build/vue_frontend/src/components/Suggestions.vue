<template>
  <div class="main">
    <div class="container-fluid">
      <div class="row mt-5">
        <div
          class="col-xl-3 col-lg-4 col-md-4 col-sm-12 col-12 pt-3"
          v-for="location in getSug"
          v-bind:key="location"
        >
          <SuggestionObject/>
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
      url: process.env.VUE_APP_BASE_BACKEND_ROOT,
    };
  },
  created() {
    this.InitiateAuth(this.GetSuggestions);
  },
  computed: mapState({
    getSug: (state) => state.suggestions,
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
    },
    async GetSuggestions(instance) {
      if (!this.$auth.isAuthenticated) return;
      await instance.getTokenSilently().then((authToken) => {
        // do authorized API calls with auth0 authToken here

        axios
          .get(`${this.url}/search/suggestions`, {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          })
          .then((data) => {
            this.$store.commit("SET_Suggestions", data);
          })
          .catch((err) => console.log(err));
      });
    },
  },
};
</script>

<style>
body {
  background-color: var(--primary-variant);
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

.card {
  border: 1px;
  box-shadow: 1px 2px 7px 6px rgba(0, 0, 0, 0.25);

  transition: 0.5s;
}

.container-fluid {
  padding-right: 50px;
  padding-left: 50px;
  margin-right: auto;
  margin-left: auto;
}
</style>
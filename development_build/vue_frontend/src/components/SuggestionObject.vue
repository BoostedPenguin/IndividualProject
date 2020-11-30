<template>
  <div class="card">
    <a class="stretched-link text-decoration-none" href="#">
      <img
        src="../assets/EiffelTower.jpg"
        class="card-img-top gallery-cover"
        alt="..."
      />
    </a>

    <div class="card-body">
      <h5 class="card-title">
        <div class="d-flex justify-content-between">
          <div>{{ $store.state.suggestions.data[0] }}</div>
          <div></div>
        </div>
      </h5>
      <div class="card-text d-flex justify-content-between">
        <p class="out-of-stock" v-if="getCurrent == null">
          Rating: unknown
        </p>
        <p class="out-of-stock" v-else>Rating: {{ getCurrent }}</p>
        <div>Description</div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mapState } from 'vuex';

export default {
  data() {
    return {
      google_key: process.env.VUE_APP_GOOGLE_KEY,
      s_guid: this.guid,
    };
  },
  created() {
      console.log(this.$store.state.suggestions.data)
  },
  computed: mapState({
    getCurrent: state => state.getSuggestion
  }),
  props: [
    'guid'
  ],
  methods: {
    async GetGooglePhotos(photo_id) {
      axios
        .get(
          `https://maps.googleapis.com/maps/api/place/photo?photoreference=${photo_id}&maxwidth=100&${this.google_key}`
        )
        .then();
    },
  },
};
</script>

<style>
</style>
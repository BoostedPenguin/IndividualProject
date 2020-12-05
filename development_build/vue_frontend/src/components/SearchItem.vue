<template>
  <div class="main-search-item">
    <div class="container-fluid pt-5">
      <div class="row">
        <div class="col-12 col-lg-4">
          <img :src="photoReference" class="img-fluid main-photo" alt="..." />
        </div>
        <div class="col-12 col-lg-8 search-item-body mt-lg-0 mt-4">
          <p>{{ searchItem.city }}</p>
          <p>{{ searchItem.country }}</p>
          <p>{{ searchItem.international_phone_number }}</p>
          <p>{{ searchItem.name }}</p>

          <p>Open now: {{ searchItem.openNow }}</p>
          <p>{{ searchItem.rating }}</p>
          <p>{{ searchItem.user_ratings_total }}</p>
          <p>{{ searchItem.vicinity }}</p>
          <p>{{ searchItem.website }}</p>

          <div class="dropdown">
            <button
              class="btn btn-primary dropdown-toggle"
              type="button"
              id="dropdownMenuButton"
              data-toggle="dropdown"
              aria-haspopup="true"
              aria-expanded="false"
            >
              Open hours
            </button>
            <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
              <a
                v-for="l in searchItem.weekdayText"
                v-bind:key="l"
                class="dropdown-item"
                href="#"
                >{{ l }}</a
              >
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
export default {
  data() {
    return {
      photoReference: "",
      google_key: process.env.VUE_APP_GOOGLE_KEY,
    };
  },

  mounted() {
    this.photoReference = `https://maps.googleapis.com/maps/api/place/photo?photoreference=${this.searchItem.photoReference}&maxwidth=500&key=${this.google_key}`;
  },

  computed: mapState({
    searchItem: (state) => state.searchItem.data,
  }),
};
</script>

<style>
.main-photo {
  max-height: 500px;
  -webkit-box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
  box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
}
.search-item-body {
  word-wrap: break-word;
  border: 1px solid black;
  border-radius: 0.5rem;
  box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
}
.main-search-item {
  min-height: 600px;
  height: 1vh;
  background-color: rgb(255, 255, 255);
}
</style>
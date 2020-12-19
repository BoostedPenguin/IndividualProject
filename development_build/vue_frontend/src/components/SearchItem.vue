<template>
  <div class="main-search-item">
    <div class="container-fluid pt-5 mb-5">
      <div v-if="searchItem" class="row">
        <div class="col-12 col-lg-4">
          <img :src="photoReference" class="img-fluid main-photo" alt="..." />
        </div>

        <div class="search-item-body col-12 col-lg-8 mt-lg-0 mt-4">
          <div class="row m-lg-5 my-2">
            <div class="col-lg-6 col-12">
              <p v-show="searchItem.name">
                <strong>Location Name:</strong> {{ searchItem.name }}
              </p>
              <p v-show="searchItem.city">
                <strong>City:</strong> {{ searchItem.city }}
              </p>
              <p v-show="searchItem.country">
                <strong>Country:</strong> {{ searchItem.country }}
              </p>
              <p v-show="searchItem.international_phone_number">
                <strong>Phone:</strong>
                {{ searchItem.international_phone_number }}
              </p>

              <p v-show="searchItem.rating">
                <strong>Rating:</strong> {{ searchItem.rating }} / 5 ({{
                  searchItem.user_ratings_total
                }}
                reviews)
              </p>
              <p v-show="searchItem.vicinity">
                <strong>Address:</strong> {{ searchItem.vicinity }}
              </p>
            </div>
            <div class="col-lg-6 col-12">
              <p v-show="searchItem.website">
                <strong>Location website:</strong>
                <a :href="searchItem.website"> {{ searchItem.website }} </a>
              </p>
              <p v-show="searchItem.openNow">
                <strong>Open now:</strong> {{ searchItem.openNow }}
              </p>

              <div v-show="searchItem.weekdayText > 0" class="dropdown">
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
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
import axios from "axios";

export default {
  data() {
    return {
      photoReference: "",
    };
  },

  created() {
    if (
      this.searchItem == null ||
      this.searchItem.placeId != this.$route.params.placeId
    ) {
      console.log(
        `Please request a new query using this placeId: ${this.$route.params.placeId}`
      );
      this.RequestSearchItem();
    } else {
      console.log(`Correct url for place :)`);
    }
  },

  mounted() {
    if (this.searchItem) {
      this.photoReference = `https://maps.googleapis.com/maps/api/place/photo?photoreference=${this.searchItem.photoReference}&maxwidth=500&key=${this.$store.state.google_key}`;
    }
  },

  computed: mapState({
    searchItem: (state) => state.searchItem.data,
  }),

  methods: {
    async RequestSearchItem() {
      await axios
        .get(
          `${this.$store.state.base_url}/search/placeid/${this.$route.params.placeId}`
        )
        .then((data) => {
          this.$store.commit("SET_SearchItem", data);
          this.photoReference = `https://maps.googleapis.com/maps/api/place/photo?photoreference=${this.searchItem.photoReference}&maxwidth=500&key=${this.$store.state.google_key}`;
        })
        .catch((error) => {
          this.error = error;
        });
    },
  },
};
</script>

<style scoped>
.search-item-main-name {
  font-size: 2em;
}
.main-photo {
  max-height: 500px;
  -webkit-box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
  box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
}
.search-item-body {
  word-wrap: break-word;
  border-radius: 0.5rem;
  box-shadow: 0px 0px 24px 2px rgba(0, 0, 0, 0.54);
}
.main-search-item {
  display: flex;
  background-color: rgb(255, 255, 255);
}
</style>
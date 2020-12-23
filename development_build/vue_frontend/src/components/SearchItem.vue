<template>
  <div class="main-search-item">
    <div class="container-fluid pt-5 mb-5">
      <!-- Alert error -->
      <transition name="basic-fade">
        <div class="alert alert-error mt-1" v-show="error" role="alert">
          {{ error }}
        </div>
      </transition>
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
            <div v-if="$auth.isAuthenticated">
              <div
                v-if="!searchItem.alreadyInWishlist"
                class="col-12 text-center"
              >
                <button
                  v-on:click="AddToWishlist()"
                  class="btn btn-add-wishlish btn-block mb-3"
                >
                  Add to wishlist
                </button>
              </div>
              <div v-else-if="searchItem.alreadyInWishlist">
                <small>This location is already in your wishlist.</small>
              </div>
            </div>

            <div v-else>
              <div class="col-12 text-center mt-3">
                <small>Login in order to add items to your wishlist.</small>
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
import { getInstance } from "../auth/authWrapper";

export default {
  data() {
    return {
      photoReference: "",
      error: "",
    };
  },

  created() {
    if (
      this.searchItem == null ||
      this.searchItem.placeId != this.$route.params.placeId
    ) {
      this.InitiateAuth(this.RequestSearchItem);
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
    searchItem: (state) => state.searchItem,
  }),

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

    InitiateAuth(fn) {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          fn(instance);
        }
      });

      if (instance.loading == false) {
        this.RequestSearchItem(instance);
      }
    },

    async RequestSearchItem(instance) {
      if (!this.$auth.isAuthenticated) {
        await this.CallSearchItem("");
      }
      await instance.getTokenSilently().then((authToken) => {
        // do authorized API calls with auth0 authToken here
        this.CallSearchItem(authToken);
      });
    },

    async CallSearchItem(authToken) {
      axios
        .get(
          `${this.$store.state.base_url}/search/placeid/${this.$route.params.placeId}`,
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          console.log(data.data);
          this.$store.commit("SET_SearchItem", data.data);
          this.photoReference = `https://maps.googleapis.com/maps/api/place/photo?photoreference=${this.searchItem.photoReference}&maxwidth=500&key=${this.$store.state.google_key}`;
        })
        .catch((error) => {
          this.error = error;
        });
    },

    async AddToWishlist() {
      let authToken = await this.ValidateUser();

      await axios
        .patch(
          `${this.$store.state.base_url}/wishlist/add`,
          {
            name: this.searchItem.name,
            lang: this.searchItem.latitude,
            long: this.searchItem.longitude,
            placeId: this.searchItem.placeId,
          },
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          this.$store.commit("SET_WishlistItems", data.data);
          this.$store.commit("SET_SearchItemInWishlist", true);
        })
        .catch((error) => {
          this.error = error.response.data;
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
.btn-add-wishlish {
  background-color: black;
  color: white;
}
</style>
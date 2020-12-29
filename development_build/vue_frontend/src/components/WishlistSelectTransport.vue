<template>
  <div class="container">
    <div class="row">
      <div class="mt-5 justify-content-start">
        <button
          id="btn-back"
          class="btn"
          @click="$emit('update:set_transport_page', false)"
        >
          Go back
        </button>
      </div>
      <div class="col-12 mt-5 text-center">
        <h3>Trip name</h3>
      </div>
      <input
        class="form-control mt-3"
        placeholder="My awesome trip!"
        v-model="name"
      />
    </div>

    <!-- Alert error -->
    <transition name="basic-fade">
      <div class="alert alert-error" v-show="error" role="alert">
        {{ error }}
      </div>
    </transition>
    <div class="row mt-5">
      <div class="col-lg-4 col-12 mt-3">
        <div class="col-lg-12 offset-lg-0 col-8 offset-2">
          <b-card
            @click="SelectCard('WALKING')"
            v-bind:class="{ 'selected-card': transportation == 'WALKING' }"
            bg-variant="primary"
            text-variant="white"
            class="text-center wishlist-card"
          >
            <div class="row justify-content-center">
              <div class="col-10">
                <img
                  class="wishlist-pictures img-fluid"
                  src="../assets/person.png"
                  alt="Card image cap"
                />
              </div>
            </div>

            <div class="card-body">
              <p class="card-text">Walking</p>
            </div>
          </b-card>
        </div>
        <hr class="hr-black" />
      </div>
      <div class="col-lg-4 col-12 mt-3">
        <div class="col-lg-12 offset-lg-0 col-8 offset-2">
          <b-card
            @click="SelectCard('BIKING')"
            v-bind:class="{ 'selected-card': transportation == 'BICYCLING' }"
            bg-variant="primary"
            text-variant="white"
            class="text-center wishlist-card"
          >
            <div class="row justify-content-center">
              <div class="col-10">
                <img
                  class="wishlist-pictures img-fluid"
                  src="../assets/bike.png"
                  alt="Card image cap"
                />
              </div>
            </div>
            <div class="card-body">
              <p class="card-text">Bike</p>
            </div>
          </b-card>
        </div>
        <hr class="hr-black" />
      </div>

      <div class="col-lg-4 col-12 mt-3 mb-3 mb-lg-0">
        <div class="col-lg-12 offset-lg-0 col-8 offset-2">
          <b-card
            @click="SelectCard('DRIVING')"
            v-bind:class="{ 'selected-card': transportation == 'DRIVING' }"
            bg-variant="primary"
            text-variant="white"
            class="text-center wishlist-card"
          >
            <div class="row justify-content-center">
              <div class="col-10">
                <img
                  class="wishlist-pictures img-fluid"
                  src="../assets/car.png"
                  alt="Card image cap"
                />
              </div>
            </div>
            <div class="card-body">
              <p class="card-text">Car</p>
            </div>
          </b-card>
        </div>
        <hr class="hr-black" />
      </div>
      <button class="btn btn-primary btn-block mt-5" @click="CreateTrip()">
        Create trip
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  props: ["set_transport_page"],
  data() {
    return {
      transportation: "",
      name: "",
      error: "",
    };
  },
  methods: {
    SelectCard(transportation) {
      this.transportation = transportation;
    },
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
    async CreateTrip() {
      if (!this.name)
        return (this.error = "You must specify a name for your trip!");
      if (!this.transportation)
        return (this.error = "You must choose a transportation method!");

      let authToken = await this.ValidateUser();

      axios
        .post(
          `${this.$store.state.base_url}/wishlist/create`,
          {
            name: this.name,
            transportation: this.transportation,
          },
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          this.$store.commit("SET_WishlistItems", null);
          console.log(data.data);
        })
        .catch((err) => {
          console.log(err);
          this.error = err;
        });
    },
  },
};
</script>

<style>
.selected-card {
  transform: scale(1.2);
  border: 5px solid black !important;
  background-color: brown !important;
}
.wishlist-card {
  cursor: pointer;
  border-radius: 1.5rem;
  border: 1px solid black;
  font-size: calc(10px + 1vw);

  transition: 0.5s;
}
.wishlist-pictures {
  max-height: 100px;
}
.wishlist-card:hover,
.wishlist-card:focus {
  color: #fff;
  transform: scale(1.1);
}

.hr-black {
  background-color: black;
}
</style>
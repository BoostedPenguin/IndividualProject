<template>
  <!-- Navigation -->
  <div>
    <b-navbar toggleable="lg" id="navbar-custom">
      <div class="container">
        <!-- Logo -->
        <router-link id="navbar-button" to="/">
          <img src="../assets/MainLogo.png" width="150" alt="" />
        </router-link>

        <!-- Logged in options -->

        <!-- My account collapse -->
        <b-navbar-toggle
          id="navbar-toggler"
          v-if="$auth.isAuthenticated"
          target="nav-collapse"
        >
          <span class="navbar-toggler-icon">
            <i
              class="fa fa-navicon"
              style="color: #fff; font-size: 28px"
            ></i> </span
        ></b-navbar-toggle>

        <b-collapse id="nav-collapse" is-nav v-if="$auth.isAuthenticated">
          <b-navbar-nav class="ml-auto flex-nowrap">
            <div class="separator" />

            <!-- My account -->
            <li class="nav-item">
              <router-link
                id="navbar-button"
                class="nav-link m-2 menu-item"
                to="/account"
                data-toggle="collapse"
                data-target=".navbar-collapse.show"
                ><i class="fa fa-user-circle-o" aria-hidden="true"></i>
                &nbsp;My Account
              </router-link>
            </li>

            <div class="separator" />

            <!-- My trips -->
            <li class="nav-item">
              <router-link
                id="navbar-button"
                class="nav-link m-2 menu-item"
                :to="{ name: 'UserTripsView' }"
                data-toggle="collapse"
                data-target=".navbar-collapse.show"
              >
                <i class="fa fa-globe" aria-hidden="true"></i>
                My Trips
              </router-link>
            </li>

            <div class="separator" />

            <!-- Wishlist options -->

            <li class="nav-item">
              <a
                id="navbar-button"
                class="nav-link m-2 menu-item"
                href="#"
                v-b-toggle.sidebar-1
                data-toggle="collapse"
                data-target=".navbar-collapse.show"
              >
                <i class="fa fa-heart" aria-hidden="true"></i>
                Wishlist
              </a>
            </li>

            <div class="separator" />

            <!-- Sign out -->
            <li class="nav-item">
              <div v-if="!$auth.loading">
                <!-- show login when not authenticated -->
                <a
                  id="navbar-button"
                  href="#"
                  class="nav-link m-2 menu-item"
                  data-toggle="collapse"
                  data-target=".navbar-collapse.show"
                  v-if="$auth.isAuthenticated"
                  @click="logout"
                >
                  <i class="fa fa-sign-out" aria-hidden="true"></i>

                  Sign Out
                </a>
              </div>
            </li>
          </b-navbar-nav>
        </b-collapse>

        <!-- Guest options -->

        <div v-if="!$auth.isAuthenticated">
          <ul class="navbar-nav ml-auto">
            <li class="nav-item">
              <div v-if="!$auth.loading">
                <!-- show login when not authenticated -->
                <a
                  href="/login"
                  id="navbar-button"
                  class="nav-link m-2 menu-item"
                  v-if="!$auth.isAuthenticated"
                  @click="login"
                >
                  Log in
                </a>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </b-navbar>

    <!-- Wishlist -->
    <div>
      <b-sidebar id="sidebar-1" title="Wishlist" right shadow>
        <div class="px-3 py-2">
          <p>Your locations:</p>
          <div v-if="getWishlist != null">
            <div v-for="w in getWishlist.locations" :key="w.id">
              <span
                class="boxed-x px-1"
                v-on:click="RemoveWishlistItem(w.id, w.placeId)"
              >
                <i class="fa fa-times" aria-hidden="true"></i>
              </span>
              <router-link
                id="wishlist-link"
                :to="{ name: 'SearchItemView', params: { placeId: w.placeId } }"
              >
                {{ w.name }}
              </router-link>
              <hr />
            </div>
            <div
              v-if="getWishlist.locations && getWishlist.locations.length > 1"
            >
              <b-link
                :to="{ name: 'WishlistPreview' }"
                class="text-decoration-none"
              >
                <button class="btn btn-primary btn-block">Create trip</button>
              </b-link>
            </div>

            <div v-else>
              <small
                >You need to add at least 2 locations to create a trip.</small
              >
            </div>
          </div>
        </div>
      </b-sidebar>
    </div>
    <transition-handler>
      <router-view :key="$route.fullPath" />
    </transition-handler>
  </div>
</template>

<script>
import { mapState } from "vuex";
import TransitionHandler from "./TransitionHandler.vue";
import { getInstance } from "../auth/authWrapper";
import axios from "axios";

export default {
  components: {
    TransitionHandler,
  },
  data() {
    return {
      error: "",
    };
  },
  computed: mapState({
    getWishlist: (state) => state.wishlist,
  }),
  created() {
    this.InitiateAuth();
  },
  methods: {
    // Log the user in
    login() {
      this.$auth.loginWithRedirect();
    },
    // Log the user out
    logout() {
      // Remove suggestions from this person..
      //this.$store.commit('SET_Suggestions', null)
      this.$auth.logout({
        returnTo: window.location.origin,
      });
    },
    InitiateAuth() {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          this.GetWishList(instance);
          this.ValidateAdmin();
        }
      });

      if (instance.loading == false) {
        this.GetWishList(instance);
        this.ValidateAdmin();
      }
    },
    async ValidateAdmin() {
      if (!this.$auth.isAuthenticated) return;
      let authToken = await this.$auth.getTokenSilently();

      axios
        .get(`${this.$store.state.base_url}/user`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_UserRole", data.data);
        })
        .catch((err) => (this.error = err.response.data));
    },
    async GetWishList() {
      if (!this.$auth.isAuthenticated) return;
      let authToken = await this.$auth.getTokenSilently();

      axios
        .get(`${this.$store.state.base_url}/wishlist`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_WishlistItems", data.data);
        })
        .catch((err) => (this.error = err.response.data));
    },
    async ValidateUser() {
      let authToken = "";
      try {
        authToken = await this.$auth.getTokenSilently();
      } catch (err) {
        console.log("Person ain't logged");
        this.error = "You aren't logged in!";
        return;
      }
      return authToken;
    },
    async RemoveWishlistItem(item_id, placeId) {
      let authToken = await this.ValidateUser();

      axios
        .patch(
          `${this.$store.state.base_url}/wishlist/remove/${item_id}`,
          null,
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          this.$store.commit("SET_WishlistItems", data.data);
          this.$store.commit("SET_SearchItemInWishlist", {
            placeId: placeId,
            isAlreadyInWishlist: false,
          });
        })
        .catch((err) => (this.error = err.response.data));
    },
  },
};
</script>

<style>
/* Modify the backgorund color */
#navbar-toggler {
  border: 1px white solid;
}
#navbar-brand:hover {
  box-shadow: 0px 0px 10px 1px rgba(0, 0, 0, 0.4);
  transition: 0.5s;
}
#navbar-custom {
  background-color: var(--penguin-primary);
}

.navbar-button:hover {
  transition: 0.3s;
  color: rgb(180, 180, 180);
}

#navbar-button {
  color: white;
  font-size: 20px;
  transition-duration: 0.4s;
}

#navbar-button:hover {
  transform: scale(1.1);
}
.boxed-x {
  border: 1px solid black;
  transition: 0.5s;
}
.boxed-x:hover,
.boxed-x:focus {
  box-shadow: 0 0 3pt 1pt #000000;
  cursor: pointer;
}

#wishlist-link {
  text-decoration: none;
  color: black;
  transition: 0.4s;
}

#wishlist-link:hover {
  text-decoration: underline;
  color: gray;
}

.separator {
  border-left: 2px solid rgb(149, 216, 255);
}
</style>
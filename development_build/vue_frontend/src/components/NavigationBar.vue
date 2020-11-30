<template>
  <!-- Navigation -->
  <nav class="navbar navbar-expand-lg navbar-custom">
    <div class="container">
      <!-- Logo -->
      <a class="navbar-brand" href="#">
        <img src="../assets/MainLogo.png" width="150" alt="" />
      </a>

      <!-- Logged in options -->

      <!-- Toggler button on small screens -->
      <button
        v-if="$auth.isAuthenticated"
        class="navbar-toggler border"
        type="button"
        data-toggle="collapse"
        data-target="#navbarResponsive"
        aria-controls="navbarResponsive"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon">
          <i class="fa fa-navicon" style="color: #fff; font-size: 28px"></i>
        </span>
      </button>

      <!-- My account collapse -->
      <div
        class="collapse navbar-collapse"
        id="navbarResponsive"
        v-if="$auth.isAuthenticated"
      >
        <ul class="navbar-nav ml-auto flex-nowrap">
          <div class="separator" />

          <!-- My account -->
          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item navbar-button" to="#"
              ><span class="fa fa-sign-in" aria-hidden="true"></span>
              &nbsp;My Account
            </router-link>
          </li>

          <div class="separator" />

          <!-- My trips -->
          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item navbar-button" to="#">
              <i class="fa fa-globe" aria-hidden="true"></i>
              My Trips
            </router-link>
          </li>

          <div class="separator" />

          <!-- Wishlist options -->

          <li class="nav-item">
            <router-link
              class="nav-link m-2 menu-item navbar-button"
              to="/about"
            >
              <i class="fa fa-heart" aria-hidden="true"></i>
              Wishlist
            </router-link>
          </li>

          <div class="separator" />

          <!-- REMOVE -->
          <li class="nav-item">
            <div v-if="!$auth.loading">
              <!-- show login when not authenticated -->
              <a
                href="#"
                class="nav-link m-2 menu-item navbar-button"
                v-if="$auth.isAuthenticated"
                @click="logout"
              >
                Sign Out
              </a>
            </div>
          </li>
          <!-- REMOVE -->

        </ul>
      </div>

      <!-- Guest options -->

      <div v-if="!$auth.isAuthenticated">
        <ul class="navbar-nav ml-auto">
          <li class="nav-item">
            <div v-if="!$auth.loading">
              <!-- show login when not authenticated -->
              <a
                href="/login"
                class="nav-link m-2 menu-item navbar-button"
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
  </nav>
</template>

<script>
export default {
  data() {
    return {};
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
  },
};
</script>

<style>
/* Modify the backgorund color */
.nav-item {
  font-size: 20px;
}

.navbar-brand:hover {
  box-shadow: 0px 0px 10px 1px rgba(0, 0, 0, 0.4);
  transition: 0.5s;
}
.navbar-custom {
  background-color: var(--primary);
}

.navbar-button:hover {
  transition: 0.3s;
  color: rgb(180, 180, 180);
}

.navbar-button {
  color: white;
}

.separator {
  border-left: 2px solid black;
}
</style>
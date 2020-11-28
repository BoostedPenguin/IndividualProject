<template>

  <!-- Navigation -->
  <nav class="navbar navbar-expand-md navbar-custom">
    <div class="container">
      <!-- Logo -->
      <a class="navbar-brand" href="#">
        <img src="../assets/MainLogo.png" width="150" alt="" />
      </a>

      <!-- Logged in options -->

      <!-- Toggler button on small screens -->
      <button
        v-if="!$auth.isAuthenticated"
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
      <div class="collapse navbar-collapse" id="navbarResponsive">
        <ul class="navbar-nav ml-auto flex-nowrap">
          <div class="separator" />

          <!-- My account -->
          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item text-white" to="#"
              ><span class="fa fa-sign-in" aria-hidden="true"></span>
              &nbsp;My Account
            </router-link>
          </li>

          <div class="separator" />

          <!-- My trips -->
          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item text-white" to="#">
              <i class="fa fa-globe" aria-hidden="true"></i>
              My Trips
            </router-link>
          </li>

          <div class="separator" />

          <!-- Wishlist options -->

          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item text-white" to="/about">
              <i class="fa fa-heart" aria-hidden="true"></i>
              Wishlist
            </router-link>
          </li>
        </ul>
      </div>

      <!-- Guest options -->

      <div v-if="$auth.isAuthenticated">
        <ul class="navbar-nav ml-auto">
          <li class="nav-item">
            <router-link class="nav-link m-2 menu-item text-white" to="/about"
              >Sign in</router-link
            >
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
};
</script>

<style>
/* Modify the backgorund color */
.nav-item {
  font-size: 20px;
}
.navbar-custom {
  background-color: #0061b5;
}

.separator {
  border-left: 2px solid black;
}
</style>
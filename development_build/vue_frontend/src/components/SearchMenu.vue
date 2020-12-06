<template>
  <div>
    <div class="main-search">
      <div class="h-100 d-flex justify-content-center align-items-center">
        <div class="col-10 col-sm-8 col-md-6 col-lg-5 col-xl-4">
          <p class="text-center search-header outline">Choose a destination</p>

          <!-- Input group -->
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <button
                class="btn filter-button"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="fa fa-caret-down" aria-hidden="true"></i>
              </button>

              <ul class="dropdown-menu checkbox-menu allow-focus">
                <li>
                  <label @click.stop="stopTheEvent">
                    <input type="checkbox" /> Filter Item 1
                  </label>
                </li>
                <li>
                  <label @click.stop="stopTheEvent">
                    <input type="checkbox" /> Filter Item 2
                  </label>
                </li>
              </ul>
            </div>
            <input
              maxlength="40"
              v-model="search_string"
              type="text"
              class="form-control form-rounded"
              placeholder="Eiffel Tower, Paris"
              aria-label="Eiffel Tower, Paris"
              aria-describedby="basic-addon2"
              @keyup.enter="Search"
            />

            <div class="input-group-append">
              <button
                v-bind:class="{ disabled: loading }"
                class="btn btn-primary search-button"
                @click="Search"
              >
                Search
                <span
                  v-bind:class="{ 'd-none': !loading }"
                  class="spinner-border spinner-border-sm"
                  role="status"
                  aria-hidden="true"
                ></span>
              </button>
            </div>
          </div>

          <!-- Alert error -->
          <transition name="basic-fade">
            <div class="alert alert-error" v-show="error" role="alert">
              {{ error }}
            </div>
          </transition>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      loading: false,
      error: "",
      search_string: "",
      url: process.env.VUE_APP_BASE_BACKEND_ROOT,
    };
  },
  methods: {
    stopTheEvent: (event) => event.stopPropagation(),

    // Executes get request for search location and displays results in another component
    async Search() {
      if (this.search_string && !this.loading) {
        this.error = "";
        this.loading = true;

        let authToken = "";
        try {
          authToken = await this.$auth.getTokenSilently();
        } catch (err) {
          // Person is a guest
        }

        await axios
          .get(`${this.url}/search/${this.search_string}`, {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          })
          .then((data) => {
            this.$store.commit("SET_SearchItem", data);
            console.log(data.data.placeId);
            this.$router.push({
              name: "SearchItemView",
              params: { placeId: data.data.placeId },
            });
          })
          .catch((error) => {
            this.error = error;
          });
        this.loading = false;
      } else {
        this.error = "Please, specify a destination first";
      }
    },
  },
};
</script>

<style>
.main-search {
  min-height: 600px;
  height: 1vh;
  background-image: url("../assets/main-background.png");
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
}

.form-rounded {
  border-radius: 1rem;
}

.filter-button {
  border-color: var(--secondary);
  background-color: var(--secondary-variant);
}

.search-button {
  border-top-right-radius: 1rem;
  border-bottom-right-radius: 1rem;
}

.filter-button:hover {
  background-color: #b55400;
}

.search-header {
  font-size: calc(15px + 1vw);
}

.outline {
  color: #fff;
  text-shadow: #000 0px 0px 5px;
  -webkit-font-smoothing: antialiased;
}

/* Dropdown checkbox styling */
.checkbox-menu li label {
  display: block;
  padding: 3px 10px;
  clear: both;
  font-weight: normal;
  line-height: 1.42857143;
  color: #333;
  white-space: nowrap;
  margin: 0;
  transition: background-color 0.4s ease;
}
.checkbox-menu li input {
  margin: 0px 5px;
  top: 2px;
  position: relative;
}

.checkbox-menu li.active label {
  background-color: #cbcbff;
  font-weight: bold;
}

.checkbox-menu li label:hover,
.checkbox-menu li label:focus {
  background-color: #f5f5f5;
}

.checkbox-menu li.active label:hover,
.checkbox-menu li.active label:focus {
  background-color: #b8b8ff;
}
</style>
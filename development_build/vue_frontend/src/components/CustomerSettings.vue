<template>
  <div class="main-content">
    <div class="container">
      <div class="row mt-5">
        <div class="col-4">
          <b-link id="btn-back" class="btn" to="/account">
            <i class="fa fa-arrow-left" aria-hidden="true">&nbsp; Back</i>
          </b-link>
        </div>
        <div class="col-4 text-center">
          <h1>Settings</h1>
        </div>
      </div>
      <div class="row mt-5">
        <div class="col-12 mt-3">
          <p>
            Automatic generation of keywords &nbsp;
            <i
              class="fa fa-question-circle"
              v-tooltip="{
                content:
                  'If you want to manually add each keyword instead of using our algorithm to determine the best ones for you, you can disable it.',
                trigger: 'click, hover',
              }"
              aria-hidden="true"
            ></i>
          </p>
          <toggle-button
            @change="ToggleLoggingKeywords()"
            :sync="true"
            :value="keyword_logging"
            color="black"
            :labels="true"
          />
        </div>
        <div class="col-12 main-keywords-box mt-3">
          <p>
            <span v-for="k in keywords" :key="k.id" class="keyword-box"
              >{{ k.keyword }}
              <button
                v-on:click="RemoveKeyword(k.id)"
                class="text-decorations-none btn-keyword-remove"
              >
                <i class="fa fa-times" aria-hidden="true"></i>
              </button>
            </span>
          </p>
        </div>
        <div class="col-12 mt-3">
          <span class="pull-right text-muted font-weight-light"
            >{{ addKeyword.length }} / 50</span
          >
          <input
            class="form-control"
            v-on:keyup.enter="AddKeyword"
            v-model="addKeyword"
            maxlength="50"
          />

          <!-- Alert error -->
          <transition name="basic-fade">
            <div class="alert alert-error mt-1" v-show="error" role="alert">
              {{ error }}
            </div>
          </transition>
        </div>
        <div class="col-6 mt-3">
          <a href="#" v-on:click="ClearKeywords()" class="clear-keywords"
            >Clear all keywords</a
          >
        </div>
        <div class="col-6 mt-3 d-flex justify-content-end">
          <div class="input-group-append">
            <button
              v-bind:class="{ disabled: loading }"
              class="btn btn-primary mb-5"
              v-on:click="AddKeyword()"
            >
              Add keyword
              <span
                v-bind:class="{ 'd-none': !loading }"
                class="spinner-border spinner-border-sm"
                role="status"
                aria-hidden="true"
              ></span>
            </button>
          </div>
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
      keywords: {},
      addKeyword: "",
      loading: false,
      error: "",
      keyword_logging: true,
    };
  },
  mounted() {
    this.GetKeywords();
    this.GetLogging();
  },
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

    async GetKeywords() {
      let authToken = await this.ValidateUser();
      await axios
        .get(`${this.$store.state.base_url}/search/keywords`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          console.log(data);
          this.keywords = data.data;
        })
        .catch((error) => {
          this.error = error;
        });
    },

    async GetLogging() {
      let authToken = await this.ValidateUser();
      await axios
        .get(`${this.$store.state.base_url}/search/keywords/logging`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          console.log(data);
          this.keyword_logging = data.data;
        })
        .catch((error) => {
          this.error = error;
        });
    },

    async RemoveKeyword(id) {
      let authToken = await this.ValidateUser();

      if (this.loading) return;
      this.loading = true;

      await axios
        .get(`${this.$store.state.base_url}/search/keywords/remove/${id}`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          console.log(data);
          this.keywords = data.data;
          this.loading = false;
        })
        .catch((error) => {
          this.error = error;
          this.loading = false;
        });
    },

    async ClearKeywords() {
      let authToken = await this.ValidateUser();

      if (this.loading) return;
      this.loading = true;

      await axios
        .delete(`${this.$store.state.base_url}/search/keywords/clear`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then(() => {
          this.error = false;
          this.loading = false;
          this.keywords = {};
        })
        .catch((error) => {
          this.error = error;
          this.loading = false;
        });
    },

    async ToggleLoggingKeywords() {
      let authToken = await this.ValidateUser();

      if (this.loading) return;
      this.loading = true;
      console.log(authToken);

      await axios
        .patch(`${this.$store.state.base_url}/search/keywords/toggle`, null, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then(() => {
          this.loading = false;
          this.keyword_logging = !this.keyword_logging;
        })
        .catch((error) => {
          this.error = error;
          this.loading = false;
        });
    },

    async AddKeyword() {
      if (this.addKeyword.length > 50 || this.loading) {
        return;
      } else if (this.addKeyword.length == 0) {
        this.error = "Keyword cannot be empty!";
        return;
      }

      this.loading = true;
      let authToken = await this.ValidateUser();

      await axios
        .get(
          `${this.$store.state.base_url}/search/keywords/add/${this.addKeyword}`,
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          console.log(data);
          this.error = false;
          this.loading = false;

          this.keywords = data.data;
        })
        .catch((error) => {
          this.error = error;
          this.loading = false;
        });
    },
  },
};
</script>

<style>
.main-content {
  min-height: 600px;
  display: flex;
  background-color: rgb(255, 255, 255);
}
.btn-keyword-remove {
  color: black;
  background: none;
  border: none;
}
.btn-keyword-remove:hover {
  color: gray;
}
.clear-keywords {
  color: var(--penguin-error);
}
.keyword-box {
  display: inline-block;
  padding: 5px;
  margin: 5px;
  border: 1px gray solid;
  word-break: break-all;
}
.main-keywords-box {
  padding: 1em;
  border: 1px black solid;
  border-radius: 1rem;
}
</style>
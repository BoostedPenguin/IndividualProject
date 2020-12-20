<template>
  <div class="main-content">
    <div class="container">
      <div class="row mt-5">
        <h3 class="col-12 text-center">Settings</h3>

        <div class="col-12 mt-3">
          <p>
            Automatic generation of keywords &nbsp;
            <i class="fa fa-question-circle" aria-hidden="true"></i>
            <button
              type="button"
              class="btn btn-secondary"
              data-toggle="tooltip"
              data-placement="bottom"
              title="Tooltip on bottom"
            >
              Tooltip on bottom
            </button>
          </p>
          <toggle-button
            v-model="suggestions"
            :value="false"
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
              class="btn btn-primary"
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
      suggestions: true,
      keywords: {},
      addKeyword: "",
      loading: false,
      error: "",
    };
  },
  mounted() {
    this.GetKeywords();
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
  color: var(--error);
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
<template>
  <div class="col-12">
    <p class="h4 text-center">
      Request data from Azure database through backend.
    </p>
    <label class="mt-5">Title</label>
    <input class="form-control" type="text" v-model="data.Title" />
    <label class="mt-5">Description</label>
    <input class="form-control" type="text" v-model="data.Description" />
    <button class="btn btn-primary btn-block mt-4" @click="tesingprod">
      Create Ticket
    </button>
    <div class="row mt-4">
      <div v-if="error" class="alert alert-danger">{{ error }}</div>
      <div v-if="apiMessage" class="col-12">{{ apiMessage }}</div>
    </div>
  </div>
</template>

<style scoped>
</style>

<script>
import axios from "axios";

export default {
  name: "external-api",
  data() {
    return {
      apiMessage: "",
      error: "",
      data: {
        Title: "",
        Description: "",
      },
      jwt: "",
    };
  },
  methods: {
    async tesingprod() {
      const token = await this.$auth.getTokenSilently();


      await axios
        .get(`https://localhost:5001/api/search/Paris`, {
          headers: {
            Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
          },
        })
        .then(() => {
          //console.log(data);
        })
        .catch((error) => {

          this.error = error;
        });
    },
    async callApi() {
      // Get the access token from the auth wrapper
      const token = await this.$auth.getTokenSilently();

      // Use Axios to make a call to the API
      //https://penguinengine.azurewebsites.net/api/glossary
      await axios
        .get("https://localhost:5001/api/ticket/1", {
          headers: {
            Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.apiMessage = data.data;
          this.error = "";
        })
        .catch((error) => {
          this.apiMessage = "";
          this.error = error;
        });
    },

    async callApiPost() {
      const token = await this.$auth.getTokenSilently();

      await axios
        .put("https://localhost:5001/api/user/", this.data, {
          headers: {
            "content-type": "application/json",
            Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.apiMessage = data.data;
          this.error = "";
        })
        .catch((error) => {
          this.apiMessage = "";
          this.error = error;
        });
    },

    async callTicketsGet() {
      const token = await this.$auth.getTokenSilently();
      //https://penguinengine.azurewebsites.net/api/example
      await axios
        .get("https://localhost:5001/api/trip", {
          headers: {
            "content-type": "application/json",
            Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.apiMessage = data.data;
          this.error = "";
        })
        .catch((error) => {
          this.apiMessage = "";
          this.error = error;
        });
    },

    async createTicket() {
      const token = await this.$auth.getTokenSilently();

      await axios
        .post("https://localhost:5001/api/ticket/", this.data, {
          headers: {
            "content-type": "application/json",
            Authorization: `Bearer ${token}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.apiMessage = data.data;
          this.error = "";
        })
        .catch((error) => {
          this.apiMessage = "";
          this.error = error;
        });
    },
  },
};
</script>
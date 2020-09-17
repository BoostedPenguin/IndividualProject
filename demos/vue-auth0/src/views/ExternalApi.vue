<template>
  <div class="col-12">
    <p class="h4 text-center">Request data from Azure database through backend.</p>
    <button class="btn btn-primary btn-block mt-4" @click="callApi">Call</button>
    <div class="row mt-4">
      <div v-if="error" class="alert alert-danger">{{ error }}</div>
      <div v-if="apiMessage" class="col-12">{{ apiMessage }}</div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "external-api",
  data() {
    return {
      apiMessage: "",
      error: "",
    };
  },
  methods: {
    async callApi() {
      // Get the access token from the auth wrapper
      const token = await this.$auth.getTokenSilently();

      // Use Axios to make a call to the API
      await axios.get("https://localhost:5001/api/glossary", {
        headers: {
          Authorization: `Bearer ${token}`    // send the access token through the 'Authorization' header
        }
      })
      .then((data) => {
        this.apiMessage = data.data;
        this.error = "";
      })
      .catch(error => {
        this.apiMessage = "";
        this.error = error;
      });
    }
  }
};
</script>
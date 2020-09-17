<template>
  <div class="container text-center">
    Hello, this is the front page



        <div>
    <button @click="callApi">Call</button>
    <p>{{ apiMessage }}</p>
  </div>



  </div>
</template>

<script>
import axios from 'axios';
export default {
   data() {
    return {
      apiMessage: ""
    };
  },
  methods: {
    async callApi() {
      // Get the access token from the auth wrapper

      // Use Axios to make a call to the API
      const { data } = await axios.get("https://localhost:5001/weatherforecast", {

      });

      this.apiMessage = data;
    }
  }
};
</script>

<style>
</style>
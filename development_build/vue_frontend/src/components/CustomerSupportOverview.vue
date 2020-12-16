<template>
  <div class="main-content">
    <div class="container">
      <div class="row mt-5 justify-content-center">
        <h1>Customer Support</h1>
      </div>
      <div class="row justify-content-end">
        <button class="btn btn-primary btn-lg mr-2 mr-lg-0 create-button">
          Create Ticket
        </button>
      </div>
      <div
        v-if="getTickets != null && getTickets.length > 0"
        class="table-responsive mt-5"
      >
        <table class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Ticket</th>
              <th scope="col">Last Activity</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="tick in getTickets" v-bind:key="tick.id">
              <th>{{ tick.id }}</th>
              <td>{{ tick.title }}</td>
              <td>
                {{ DisplayDate(tick) }}
              </td>
              <td><button class="btn view-button btn-block">View</button></td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-else>
        <p>You haven't created any support tickets</p>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
import axios from "axios";
export default {
  data() {
    return {
      url: process.env.VUE_APP_BASE_BACKEND_ROOT,
    };
  },
  computed: mapState({
    getTickets: (state) => state.supportTickets.data,
  }),
  mounted() {
    this.GetSupportTickets();
  },
  methods: {
    async GetSupportTickets() {
      console.log("YO");

      const authToken = await this.$auth.getTokenSilently();

      await axios
        .get(`${this.url}/ticket`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_SupportTicket", data);
          console.log(data);
        })
        .catch((error) => {
          this.error = error;
        });
    },
    DisplayDate(tick) {
      let date;
      if (tick.updatedAt == null) {
        date = tick.createdAt;
      } else if (tick.updatedAt >= tick.createdAt) {
        date = tick.updatedAt;
      }
      let final = new Date(Date.parse(date));
      return final.toDateString();
    },
  },
};
</script>

<style scoped>
.view-button {
  background-color: var(--secondary-variant);
  color: white;
}
.create-button {
  background-color: var(--primary);
  border-radius: 1rem;
}
.main-content {
  min-height: 600px;
  display: flex;
  background-color: rgb(255, 255, 255);
}
</style>
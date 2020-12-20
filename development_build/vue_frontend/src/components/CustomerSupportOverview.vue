<template>
  <div class="main-content">
    <div class="container">
      <div class="row mt-5">
        <div class="col-4 justify-content-start">
          <router-link class="btn btn-success" to="/account">
            <i class="fa fa-arrow-left" aria-hidden="true">&nbsp; Back</i>
          </router-link>
        </div>
        <h1>Customer Support</h1>
      </div>
      <div class="row justify-content-end mt-5">
        <router-link
          to="/support/ticket/create"
          class="btn btn-primary btn-lg mr-2 mr-lg-0 create-button"
        >
          Create Ticket
        </router-link>
      </div>
      <div v-if="loadingTickets" class="text-center mt-5">
        <p class="">Fetching tickets...</p>
        <div class="spinner-border text-primary" role="status" />
      </div>
      <div v-else>
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
                <td>
                  <router-link
                    :to="{
                      name: 'TicketContentView',
                      params: { ticketid: tick.id },
                    }"
                    class="text-decoration-none"
                  >
                    <button class="btn view-button btn-block">View</button>
                  </router-link>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else>
          <p>You haven't created any support tickets</p>
        </div>
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
      loadingTickets: true,
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

      this.loadingTickets = true;
      await axios
        .get(`${this.$store.state.base_url}/ticket`, {
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
      this.loadingTickets = false;
    },
    DisplayDate(tick) {
      let date;
      if (tick.updatedAt == null) {
        date = tick.createdAt;
      } else if (tick.updatedAt >= tick.createdAt) {
        date = tick.updatedAt;
      }
      let final = new Date(Date.parse(date));
      return final.toLocaleString();
    },
  },
};
</script>

<style scoped>
.view-button {
  background-color: var(--secondary-variant);
  color: white;
}
.view-button:hover {
  background-color: var(--secondary);
}
.create-button {
  background-color: var(--primary);
  border-radius: 1rem;
}
.create-button:hover {
  background-color: var(--primary-variant);
}

.spinner-text {
  font-size: 2em;
  color: white;
}

.main-content {
  min-height: 600px;
  display: flex;
  background-color: rgb(255, 255, 255);
}
</style>
<template>
  <div class="main-content">
    <div class="container">
      <div class="row mt-5">
        <div class="col-lg-4 col-2">
          <b-link id="btn-back" class="btn" to="/account">
            <i class="fa fa-arrow-left" aria-hidden="true">&nbsp; Back</i>
          </b-link>
        </div>
        <div class="col-lg-4 col-10 text-center">
          <h1>Admin Customer Support</h1>
        </div>
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
import { getInstance } from "../auth/authWrapper";

export default {
  data() {
    return {
      loadingTickets: true,
    };
  },
  computed: mapState({
    getTickets: (state) => state.supportTickets.data,
  }),
  created() {
    this.InitiateAuth();
  },
  methods: {
    InitiateAuth() {
      // have to do this nonsense to make sure auth0Client is ready
      var instance = getInstance();

      instance.$watch("loading", (loading) => {
        if (loading === false) {
          this.AdminGetAllTickets();
        }
      });

      if (instance.loading == false) {
        this.AdminGetAllTickets();
      }
    },
    async AdminGetAllTickets() {
      const authToken = await this.$auth.getTokenSilently();

      this.loadingTickets = true;
      await axios
        .get(`${this.$store.state.base_url}/ticket/admin`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.$store.commit("SET_SupportTicket", data);
          console.log(data);
        })
        .catch((error) => {
          this.error = error.response.data;
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
  background-color: var(--penguin-secondary-variant);
  color: white;
}
.view-button:hover {
  background-color: var(--penguin-secondary);
}
.create-button {
  background-color: var(--penguin-primary);
  border-radius: 1rem;
}
.create-button:hover {
  background-color: var(--penguin-primary-variant);
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
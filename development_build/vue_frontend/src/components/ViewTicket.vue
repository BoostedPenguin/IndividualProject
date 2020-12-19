<template>
  <div class="main-content">
    <div class="container">
      <div class="row">
        <div class="col-4 mt-5 justify-content-start">
          <router-link class="btn btn-success" to="/support">
            <i class="fa fa-arrow-left" aria-hidden="true">&nbsp; Back</i>
          </router-link>
        </div>
        <div class="col-8 mt-5 justify-content-center">
          <h3>Ticket Support</h3>
        </div>
      </div>
      <div class="row my-3 main-border">
        <div class="col-12 p-5">
          <div class="col-12"><strong>Title:</strong> {{ ticket.title }}</div>
          <div class="col-12">
            <strong>Created at:</strong> {{ DisplayDate(ticket.createdAt) }}
          </div>
          <div class="col-12 mt-2 border">
            <strong>Description</strong> {{ ticket.description }}
          </div>
          <hr class="hr-black" />
        </div>

        <div class="col-12 px-5">
          <div
            class="m-3"
            v-bind:class="{ 'text-right': chat.isCurrentUser }"
            v-for="chat in ticket.ticketChat"
            v-bind:key="chat.id"
          >
            <div>
              <small>
                {{ chat.isCurrentUser ? "Me" : "Admin" }} |
                {{ DisplayDate(chat.createdAt) }}
              </small>
            </div>
            <span class="message">
              {{ chat.message }}
            </span>
          </div>
        </div>
      </div>
      <div class="row mb-3">
        <div class="col-12">
          <span class="pull-right text-muted font-weight-light"
            >{{ message.length }} / 300</span
          >
          <textarea
            rows="2"
            v-model="message"
            maxlength="300"
            class="form-control"
            v-on:keyup.enter="SendMessage"
          />

          <!-- Alert error -->
          <transition name="basic-fade">
            <div class="alert alert-error mt-1" v-show="error" role="alert">
              {{ error }}
            </div>
          </transition>
          <button
            v-bind:class="{ disabled: loading }"
            class="btn btn-send-message text-white mt-1 pull-right"
            @click="SendMessage"
          >
            Send message
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
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      loading: false,
      error: "",
      ticket: {},
      message: "",
    };
  },
  props: ["ticketid"],
  created() {
    this.GetTicketDetails();
  },
  methods: {
    async GetTicketDetails() {
      let authToken = "";
      try {
        authToken = await this.$auth.getTokenSilently();
      } catch (err) {
        console.log("Person ain't logged");
        this.error = "You aren't logged in!";
        this.$router.push({ name: "Home" });
      }

      await axios
        .get(`${this.$store.state.base_url}/ticket/${this.ticketid}`, {
          headers: {
            Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
          },
        })
        .then((data) => {
          this.ticket = data.data;
          console.log(this.ticket);
        })
        .catch((err) => {
          console.log(err);
          this.$router.push({ name: "Home" });
        });
    },

    DisplayDate(date) {
      let final = new Date(Date.parse(date));
      return final.toLocaleString();
    },

    async SendMessage() {
      if (this.message.length > 300 || this.message.length == 0) {
        return;
      }
      this.loading = true;

      let authToken = "";
      try {
        authToken = await this.$auth.getTokenSilently();
      } catch (err) {
        console.log("Person ain't logged");
        this.error = "You aren't logged in!";
        this.$router.push({ name: "Home" });
      }

      await axios
        .post(
          `${this.$store.state.base_url}/ticket/message/${this.ticketid}`,
          {
            message: this.message,
          },
          {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          }
        )
        .then((data) => {
          this.ticket = data.data;
          this.message = "";
          console.log(this.ticket);
        })
        .catch((err) => {
          console.log(err);
          this.error = err;

          //this.$router.push("home");
        });
      this.loading = false;
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

.btn-send-message {
  background-color: var(--secondary-variant);
}

.message {
  padding: 5px;
  background-color: lightgray;
  border-radius: 0.5em;
}

.main-border {
  border: black 1px solid;
  border-radius: 1rem;
}
</style>
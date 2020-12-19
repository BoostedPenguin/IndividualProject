<template>
  <div class="main-content">
    <div class="container">
      <div class="row">
        <div class="col-4 mt-5 justify-content-start">
          <router-link class="btn btn-success" to="/support">
            <i class="fa fa-arrow-left" aria-hidden="true">&nbsp; Back</i>
          </router-link>
        </div>
        <div class="col-8 mt-5 justify-content-end">
          <h3>Create a support request</h3>
        </div>

        <div class="mt-5 col-12">
          <transition name="basic-fade">
            <div class="col-12 alert alert-danger" v-show="error" role="alert">
              {{ error }}
            </div>
          </transition>
          <label for="title">Title</label>
          <input
            name="title"
            v-model="ticket.title"
            maxlength="50"
            class="form-control"
          />
          <label for="description">Description</label>
          <span class="pull-right text-muted font-weight-light"
            >{{ ticket.description.length }} / 500</span
          >
          <textarea
            name="description"
            v-model="ticket.description"
            class="form-control"
            rows="5"
            maxlength="500"
          />
        </div>
      </div>
      <button class="btn btn-primary mt-5 btn-block" v-on:click="SendTicket">
        Create ticket
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      current_letters: 0,
      error: "",
      ticket: {
        title: "",
        description: "",
      },
    };
  },
  methods: {
    async SendTicket() {
      if (
        this.ticket.title &&
        this.ticket.description &&
        this.ticket.title.length <= 50 &&
        this.ticket.description.length <= 500
      ) {
        //Content

        let authToken = "";
        try {
          authToken = await this.$auth.getTokenSilently();
        } catch (err) {
          console.log("Person ain't logged");
          this.error = "You aren't logged in!";
          return;
        }
        await axios
          .post(`${this.$store.state.base_url}/ticket`, this.ticket, {
            headers: {
              Authorization: `Bearer ${authToken}`, // send the access token through the 'Authorization' header
            },
          })
          .then((data) => {
            console.log(data);
            this.$router.push({ name: "CustomerSupportView" });
          })
          .catch((err) => console.log(err));
      }
      if (!this.ticket.title) {
        this.error = "You need to fill in a title";
      } else if (this.ticket.title.length > 50) {
        this.error = "Title needs to be 50 characters or less";
      } else if (!this.ticket.description) {
        this.error = "You need to fill in a description";
      } else if (this.ticket.title.length > 500) {
        this.error = "Description needs to be 500 characters or less";
      }
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
</style>
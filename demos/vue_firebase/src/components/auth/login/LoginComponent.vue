<template>
  <div class="container">
    <div class="row justify-content-center mt-5">
      <div class="col-lg-6">
        <div class="card">
          <div class="card-header">
            <div class="h1 text-center">
              Login
            </div>
          </div>
          <div class="card-body">
            <form @submit.prevent="submit">
              <div class="form-group">
                <label for="email">Email address</label>
                <input required type="email" class="form-control" v-model="form.email" id="email" aria-describedby="emailHelp" placeholder="Enter email">
              </div>
              <div class="form-group">
                <label for="password">Password</label>
                <input required type="password" class="form-control" v-model="form.password" id="password" aria-describedby="emailHelp" placeholder="Enter password">
              </div>

              <div class="row justify-content-center">
                <div class="col-12 ">
                  <button type="submit" class="btn btn-primary btn-block">Submit</button>
                </div>
              </div>

            </form>
          </div>
          <div class="card-footer">
              <div class="col-12 text-center">
                Not registered? Click 
                <router-link class="font-weight-bold" to="/register">here</router-link>
                to register
              </div>
          </div>
        </div>

        <div class="row justify-content-center mt-5">
          <button @click="checkStatus" class="btn btn-primary">Check status</button>
        </div>

        <div class="row justify-content-center mt-3" v-if="logged">
          <button @click="logOut" class="btn btn-secondary">Log out</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import firebase from "firebase";


export default {
  name: "LoginComponent",
  components: {

  },
  computed: {
    ...mapGetters({
      user: "user"
    })
  },
  data() {
    return {
      current: '',
      form: {
        email: '',
        password: '',
      },

      logged: false,
    }
  },
  methods: {
    submit() {
      
      firebase.auth().signInWithEmailAndPassword(this.form.email, this.form.password)
      .then(() => {
        alert("Succesfully logged in")
        this.logged = true;
      })
      .catch((err) => {
        alert(err.message)
      })
    },

    checkStatus() {
      alert(firebase.auth().currentUser ? "Logged in" : "Not logged in")
    },

    logOut() {
      firebase.auth().signOut().then(() => {
        alert("Succesfully logged out")
        this.logged = false;
      })
      .catch((err) => {
        alert(err.message);
      })
    }
  }

}
</script>

<style scoped>
.something {
  color: red;
}
</style>
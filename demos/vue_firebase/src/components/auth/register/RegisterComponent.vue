<template>
  <div class="container">
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-header">
            Register
          </div>
          <div class="card-body">
            <div class="alert alert-success" v-if="success">
                We've sent you a verification email. Please, check your inbox.
            </div>
            <form action="#" @submit.prevent="submit">
              <!-- <div class="form-group">
                <label for="name">Name</label>
                <input required v-model="form.name" name="name" type="text" class="form-control" id="name" aria-describedby="emailHelp" placeholder="Enter name">
              </div> -->
              <div class="form-group">
                <label for="email">Email address</label>
                <input required v-model="form.email" name="email" type="email" class="form-control" id="email" aria-describedby="emailHelp" placeholder="Enter email">
              </div>
              <div class="form-group">
                <label for="password">Password</label>
                <input required v-model="form.password" name="password" type="password" class="form-control" id="password" aria-describedby="emailHelp" placeholder="Enter password">
              </div>
              <div class="alert alert-danger" v-if="error && error.message">
                  {{error}}
              </div>
              <button type="submit" class="btn btn-primary">Submit</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>

import firebase from "firebase";

export default {
  name: "LoginComponent",

  data() {
      return {
          form: {
              password: "",
              email: "",
          },

          actionCodeSettings: {
            url: 'http://localhost:8080/register/continue',
            handleCodeInApp: true,
          },

          success: false,
          error: {}
      }
  },
  methods: {
      submit() {
          firebase.auth().createUserWithEmailAndPassword(this.form.email, this.form.password).then(() => {
            firebase.auth().currentUser.sendEmailVerification()
            
            this.success = true;
            this.error = {};
          }).catch(error => {
              this.success = false;
              this.error = error;
          })

          // firebase.auth().sendSignInLinkToEmail(this.form.email, this.actionCodeSettings)
          // .then(function() {
          //   //Link was sent successfully
          //   window.localStorage.setItem('emailForSignIn', email);
          // })
          // .catch(function(error) {
          //   console.log(error);
          // })
      }
  }

}
</script>

<style scoped>
.something {
  color: red;
}
</style>
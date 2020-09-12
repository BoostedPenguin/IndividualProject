<template>
  <div class="container">

  </div>
</template>

<script>
export default {
    data() {
        return {

        }
    },
    methods: {
        Complete() {
            if(firebase.auth().isSignInWithEmailLink(window.location.href)) {
                var email = window.localStorage.getItem('emailForSignIn');
                if(!email) {
                    email = window.prompt('Please provide your email for confirmation')
                }
                firebase.auth().signInWithEmailLink(email, window.location.href)
                .then(function(result) {
                    window.localStorage.removeItem('emailForSignIn')

                    //Result contains user information
                })
                .catch(function(error) {
                    //Catch errors // error.code
                })
            }
        }
    }
}
</script>

<style>

</style>
import Vue from 'vue'
import App from './App.vue'
import * as firebase from "firebase";
import router from "./routes/index"
import store from "./store";
import axios from 'axios';

import '@/components'

Vue.config.productionTip = false

const configOptions = {
  apiKey: "AIzaSyBnsEifXEDGpDIxliF8Xe8JtfNHQv7P6wM",
  authDomain: "individualprojects3.firebaseapp.com",
  databaseURL: "https://individualprojects3.firebaseio.com",
  projectId: "individualprojects3",
  storageBucket: "individualprojects3.appspot.com",
  messagingSenderId: "970574817793",
  appId: "1:970574817793:web:a7a8e39314e550975d2e00",
  measurementId: "G-7YTW5BFM8H"
};

firebase.initializeApp(configOptions);
firebase.analytics();
firebase.auth().onAuthStateChanged(user => {
  store.dispatch("fetchUser", user);
});



new Vue({
  router,
  store,
  axios,
  render: h => h(App),
}).$mount('#app')

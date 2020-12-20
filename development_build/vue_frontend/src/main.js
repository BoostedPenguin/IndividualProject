import Vue from 'vue'
import App from './App.vue'
import router from './router'
import '../public/global.css'

import ToggleButton from 'vue-js-toggle-button'
Vue.use(ToggleButton)
// Import the Auth0 configuration
import { domain, clientId, audience } from "../auth_config.json"
import store from "./store/store"


// Import the plugin here
import { Auth0Plugin } from "./auth"

let base_url = process.env.VUE_APP_BASE_BACKEND_ROOT

Vue.use(Auth0Plugin, {
  base_url,
  domain,
  clientId,
  audience,
  onRedirectCallback: appState => {
    router.push(appState && appState.targetUrl
      ? appState.targetUrl
      : window.location.pathname
    )
  }
})

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')

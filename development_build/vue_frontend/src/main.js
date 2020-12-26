import Vue from 'vue'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import '../public/global.css'

import ToggleButton from 'vue-js-toggle-button'
import VTooltip from 'v-tooltip'

import { domain, clientId, audience } from "../auth_config.json"
import store from "./store/store"

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'

// Import the plugin here
import { Auth0Plugin } from "./auth"

Vue.use(VTooltip)
Vue.use(ToggleButton)
// Import the Auth0 configuration
// Install BootstrapVue
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)


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

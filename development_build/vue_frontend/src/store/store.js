import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)
const host = process.env.VUE_APP_BASE_BACKEND_ROOT

export default new Vuex.Store({
  state: {
    suggestions: {},
    wishlist: {},
    searchItem: {},
    supportTickets: {},
    base_url: process.env.VUE_APP_BASE_BACKEND_ROOT,
    google_key: process.env.VUE_APP_GOOGLE_KEY,
    ip_stack: process.env.VUE_APP_IP_STACK_KEY,
  },
  mutations: {
    SET_Suggestions(state, payload) {
      state.suggestions = payload
    },
    SET_SearchItem(state, payload) {
      state.searchItem = payload
    },
    SET_SupportTicket(state, payload) {
      state.supportTickets = payload
    },
    SET_WishlistItems(state, payload) {
      state.wishlist = payload
    },
    SET_SearchItemInWishlist(state, payload) {
      if (state.searchItem.placeId == 0 || payload.placeId == 0) return
      if (state.searchItem.placeId == payload.placeId || payload.placeId == null) {
        state.searchItem.alreadyInWishlist = payload.isAlreadyInWishlist
      }
    }
  },
  getters: {
    GET_SearchItem: state => {
      return state.searchItem
    },
    getCurrentSuggestions: state => {
      return state.suggestions
    },
    getSuggestion: state => (guid) => {
      return state.suggestions.data.find(sug => sug.guid = guid)
    }
  },
  actions: {
    loadSuggestions({ commit, getters }) {
      console.log(getters.token)

      if (!getters.token) {
        return console.log("Fuck my life")
      }
      axios
        .get(`${host}/search/suggestions`, {
          headers: {
            Authorization: `Bearer ${getters.token}`, // send the access token through the 'Authorization' header
          },
        })
        .then(data => data.data)
        .then(items => {
          console.log(items)
          commit('SET_Items', ...items)
        })
    }
  }
})

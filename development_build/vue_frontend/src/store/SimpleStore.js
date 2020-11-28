import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    count: 0,
    message: "",
  },
  mutations: {
    increment (state, payload) {
      state.message = payload.message,
      state.count++
    },
    decrement (state, payload) {
      state.message = payload.message,
      state.count--
    }
  },
})
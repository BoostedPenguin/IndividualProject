import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import SearchItemView from '../views/SearchItemView.vue'
import AccountView from '../views/AccountView.vue'
import CustomerSupportView from '../views/CustomerSupportView.vue'
import TicketCreationView from '../views/TicketCreationView.vue'
import { authGuard } from "../auth/authGuard"

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/search/:placeId',
    name: 'SearchItemView',
    component: SearchItemView,
    meta: { transitionName: 'slide' },
  },
  {
    path: '/account',
    name: 'Account',
    component: AccountView,
    meta: { transitionName: 'slide' },
    beforeEnter: authGuard
  },
  {
    path: '/support',
    name: 'CustomerSupportView',
    component: CustomerSupportView,
    meta: { transitionName: 'slide' },
    beforeEnter: authGuard
  },
  {
    path: '/support/ticket',
    name: 'TicketCreationView',
    component: TicketCreationView,
    meta: { transitionName: 'slide' },
    beforeEnter: authGuard
  },
  {
    path: '*',
    component: Home
  }
  // {
  //   path: '/about',
  //   name: 'About',
  //   // route level code-splitting
  //   // this generates a separate chunk (about.[hash].js) for this route
  //   // which is lazy-loaded when the route is visited.
  //   component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  // },
  // {
  //   path: "/profile",
  //   name: "profile",
  //   component: Profile,
  //   beforeEnter: authGuard
  // }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router

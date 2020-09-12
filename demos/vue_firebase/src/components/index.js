import Vue from 'vue'
import LoginComponent from '../components/auth/login/LoginComponent'
import Navbar from '../components/navbar/Navbar'
import RegisterComponent from '../components/auth/register/RegisterComponent'
import home from '../components/home'

Vue.component('login', LoginComponent)
Vue.component('navbar', Navbar)
Vue.component('register', RegisterComponent)
Vue.component('home', home)

export default {
    
}
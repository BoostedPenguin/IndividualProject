import Vue from 'vue';
import Router from 'vue-router';
import Home from '../components/home'
import Login from '../components/auth/login/LoginComponent'
import Register from '../components/auth/register/RegisterComponent'

Vue.use(Router)

const router = new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
    {
        path: '',
        name: 'index',
        component: Home
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/register',
        name: 'register',
        component: Register
    }
]
});

export default router
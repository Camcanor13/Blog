import { createRouter, createWebHistory } from 'vue-router';
import Login from './src/pages/Login.vue';

import Moderador from './src/pages/Dashboard.vue';
import Registro from './src/pages/Registro.vue';
import Dashboard from './src/pages/Dashboard.vue';

const routes = [
  { path: '/', component: Login },
  { path: '/dashboard', component: Dashboard },
  { path: '/registro', component: Registro },

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
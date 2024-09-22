import { createRouter, createWebHistory } from 'vue-router';
import Login from './src/pages/Login.vue';
import Publicaciones from './src/pages/Mispublicaciones.vue'
import Moderador from './src/pages/Dashboard.vue';
import Registro from './src/pages/Registro.vue';
import Dashboard from './src/pages/Dashboard.vue';
import Inicio from './src/pages/Publicaciones.vue'
import Perfil from './src/pages/Perfil.vue';
import Lista from './src/pages/ListaMeGusta.vue'

const routes = [
  { path: '/', component: Login },
  { path: '/registro', component: Registro} ,

  { path: '/dashboard', component: Dashboard ,
    children:[
      { path: 'inicio', component:Inicio },
      { path: 'publicaciones', component:Publicaciones }, 
      { path: 'perfil', component:Perfil }, 
      { path: 'lista', component:Lista }, 
    ]},

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
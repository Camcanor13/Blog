<template>
  <nav class="bg-white shadow-md">
    <div class="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
      <div class="flex">
        <img src="../../public/logo.png" class="w-8 h-8 p-1">
        <router-link to="/dashboard/inicio">
          <h1 class="text-xl "><span class="font-bold">La pluma </span>Digital</h1>
        </router-link>

      </div>
      <div>
        <ul class="list-none p-0 md:flex md:space-x-4">
          <li>
            <router-link to="/dashboard/inicio"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Inicio
            </router-link>
          </li>
          <li>
            <router-link to="/dashboard/about"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              About Us
            </router-link>
          </li>
          <li v-if="userRol === '1'">
            <router-link to="/dashboard/publicaciones"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Mis publicaciones
            </router-link>
          </li>
          <li v-if="userRol === '3'">
            <router-link to="/dashboard/lista"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Listado Me Gusta
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const userRol = ref('');

// Función para obtener el rol del usuario desde el localStorage
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
};

onMounted(() => {
  getUserRole();
});
</script>

<style>
/* Agrega tus estilos personalizados aquí */
</style>

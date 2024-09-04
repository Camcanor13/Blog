<template>
    <nav class="border border-black px-6 py-4 bg-white px-6 py-4 shadow">
      <div class="flex flex-1 justify-between">
      <div> <!-- Contenedor para logo o título, si es necesario -->
        <h1 class="text-xl font-semibold">Mi Aplicación</h1>
      </div>
      <div class="flex flex-col md:flex-row md:space-x-4 md:justify-end">
        <ul class="list-none p-0 flex">
          <li>
            <router-link to="/dashboard/inicio" class="my-1 text-gray-800 hover:text-blue-500 md:mx-4 md:my-0">
              Inicio
            </router-link>
          </li>
          <li>
            <router-link to="/dashboard/about" class="my-1 text-gray-800 hover:text-blue-500 md:mx-4 md:my-0">
              About Us
            </router-link>
          </li>
          <li v-if="userRol === '1'">
            <router-link to="/dashboard/publicaciones" class="my-1 text-gray-800 hover:text-blue-500 md:mx-4 md:my-0">
              Mis publicaciones
            </router-link>
          </li>
        </ul>
      </div>
    </div> 
 
     <!-- <ul >
        <li >
          <router-link to="/dashboard/inicio">Inicio</router-link>
          <router-link to="/dashboard/inicio">About Us</router-link>
        </li>
        <li v-if="userRol === '1'">
          <router-link to="/dashboard/publicaciones">Mis publicaciones</router-link>
        </li>
      </ul>
      -->
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
  /* Estilos opcionales */
  </style>
  
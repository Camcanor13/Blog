<template>
    <nav class="border border-black">
      <ul>
        <li>
          <router-link to="/dashboard/inicio">Inicio</router-link>
        </li>
        <li v-if="userRol === '1'">
          <router-link to="/dashboard/publicaciones">Mis publicaciones</router-link>
        </li>
      </ul>
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
  
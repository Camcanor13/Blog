<script setup>
import { ref } from 'vue';
import axios from 'axios';

import { useRouter } from 'vue-router';

const email = ref('');
const password = ref('');
const router = useRouter();


const login = async () => {
  try {
   
    const response = await axios.post('/api/Login', {
      email: email.value,
      password: password.value,
    });

    // Manejar la respuesta
    if (response.status === 200) {
      console.log(response);

      localStorage.setItem('userData', JSON.stringify({
        id:response.data.id,
        userName: response.data.userName, 
        rol: response.data.rol,
        email: response.data.email
      })); //almacena los datos de la persona que inicio sesion en localstorage

      alert(`Bienvenida: ${response.data.userName}`);
      router.push("/dashboard/inicio") // Mostrar mensaje de éxito
      
    } else {
      alert('Acceso denegado: ' + response.data); // Mostrar mensaje de error
    }
  } catch (error) {
    console.error('Error al iniciar sesión:', error);
    alert(error.response.data);
  }
};
</script>

<template>
  <form @submit.prevent="login" class="bg-white block w-2/4 mx-auto my-64 rounded-md p-5">
    <h1 class="text-center">Iniciar Sesión</h1>
    <div class="grid grid-cols-1 w-50 p-5">
      <label>Correo Electrónico:</label>
      <input v-model="email" type="text" placeholder="correo@correo.com" class="border rounded-md">
      
      <label>Contraseña:</label>
      <input v-model="password" type="password" placeholder="**********" class="border rounded-md">
    </div>
    <p >¿Aún no estás registrado? <router-link to="/registro">Click Aquí</router-link></p>
    <button type="submit" class="border rounded-full bg-zinc-300 px-2 py-1 hover:bg-zinc-500">Iniciar sesión</button>
   
  </form>
  
</template>

<style scoped>
/* Estilos opcionales */
</style>

<template>
    <form @submit.prevent="register" class="bg-white block w-2/4 mx-auto my-64 rounded-md p-5">
      <h1 class="text-center">Registro</h1>
      <div class="grid grid-cols-1 w-50 p-5">
        <label>Correo Electrónico:</label>
        <input v-model="email" type="text" placeholder="correo@correo.com" class="border rounded-md">
        
        <label>Contraseña:</label>
        <input v-model="password" type="password" placeholder="**********" class="border rounded-md">
        
        <label>Nombre de Usuario:</label>
        <input v-model="user" type="text" placeholder="Nombre de Usuario" class="border rounded-md">
        
        <label>Rol:</label>
        <select v-model="rol" class="border rounded-md">
          <option value="1">Autor</option>
          <option value="3">Lector</option>
        </select>
      </div>
      <button type="submit" class="border rounded-full bg-zinc-300 px-2 py-1 hover:bg-zinc-500">Registrar</button>
    </form>
    <p>¿Ya tienes una cuenta? <router-link to="/">Inicia sesión aquí</router-link></p>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  
  //campos del formulario
  const email = ref('');
  const password = ref('');
  const user = ref('');
  const rol = ref(''); 
  const router = useRouter();
  

  const register = async () => {
    try {
    
      const response = await axios.post('/api/Register', {
        email: email.value,
        password: password.value,
        user: user.value,
        rol: rol.value,
      });

      if (response.status === 200) {
        alert('Registro exitoso');
        router.push('/'); 
      } else {
        alert('Error: ' + response.data);
      }
    } catch (error) {
      console.error('Error al registrar:', error);
      alert('Error al registrar: ' + error.response.data);
    }
  };
  </script>
  
  <style scoped>
 
  </style>
  
<script setup>
import { ref } from 'vue';
import axios from 'axios';
import image from '@/assets/vue.svg';

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
        id: response.data.id,
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

  <div @submit.prevent="login" class="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-sm">
      <img class="mx-auto h-10 w-auto" src="@/assets/vue.svg" alt="Your Company" />
      <h2 class="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900">Iniciar sesion
      </h2>
    </div>
    <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
      <form class="space-y-6" action="#" method="POST">
        <div>
          <label for="email" class="block text-sm font-medium leading-6 text-gray-900">Correo Electrónico</label>
          <div class="mt-2">
            <input v-model="email" id="email" name="email" type="email" autocomplete="email"
              placeholder="correo electronico"
              class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" />
          </div>
          <div>
            <div class="flex items-center justify-between">
              <label for="password" class="block text-sm font-medium leading-6 text-gray-900">Password</label>
            </div>
            <div class="mt-2">
              <input placeholder="*******" v-model="password" id="password" name="password" type="password"
                autocomplete="current-password"
                class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" />
            </div>
          </div>
        </div>
        <div>
          <button type="submit"
            class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Iniciar
            Sesion</button>
        </div>
      </form>
    </div>

    <p class="mt-10 text-center text-sm text-gray-500">
      ¿Aún no estás registrado?
      {{ ' ' }}
      <router-link class="font-semibold leading-6 text-indigo-600 hover:text-indigo-500"
        to="/registro">Registrate</router-link>
    </p>
  </div>
</template />

<style scoped>
/* Estilos opcionales */
</style>
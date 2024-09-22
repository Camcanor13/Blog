<template>
    <h1>AQUI VA EL LISTADO</h1>
    <p>------------------------------</p>
    <ul>
        <li v-for="publicacion in publicaciones" :key="publicacion.title" class="border">
            <h1>{{ publicacion.title }}</h1>
           <p>{{ publicacion.body }}</p> 
           <h2>{{ publicacion.author }}</h2>
           <h3>{{ publicacion.date }}</h3>
        </li>
        
    </ul>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const usuario = ref("");
const publicaciones = ref([]);

const datos = localStorage.getItem("userData");
console.log(datos);
usuario.value = JSON.parse(datos).id; // Parsea el objeto JSON
console.log(usuario.value);

const getLikeStatus = async (idUsuario) => {
  try {
    const response = await axios.get(`/api/MeGusta/usuario/${idUsuario}`);
    const usersWhoLiked = response.data; // Asigna response.data

    console.log('Publicaciones a las que has dado me gusta:', usersWhoLiked);
    publicaciones.value = usersWhoLiked; // Asigna el resultado a la variable publicaciones
  } catch (error) {
    console.error('Error al obtener "Me gusta":', error.response ? error.response.data : error.message);
  }
};

onMounted(() => {
  getLikeStatus(usuario.value);
});
</script>

<style>

</style>
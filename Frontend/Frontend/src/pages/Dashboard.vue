<template>
  <Header/>

  <button v-if="visibilidad" @click="toggleAgregarPublicacion">
    Agregar Publicacion
  </button>
  
  <AgregarPublicacion v-if="mostrarAgregarPublicacion"/>

  <Publicaciones/>

</template>

<script setup>
import { ref } from 'vue';
import Header from '../components/Header.vue';
import Publicaciones from '../components/Publicaciones.vue';
import AgregarPublicacion from '../components/AgregarPublicacion.vue';

// Variables reactivas para controlar la visibilidad de componentes
const mostrarAgregarPublicacion = ref(false);
const visibilidad = ref(false); //determina apartir del tipo de rol que tengan


const userData = localStorage.getItem('userData');
if (userData) {
  const user = JSON.parse(userData);
  console.log(user);
  
  if (user.rol === "1") {  // Si el rol es "1", es decir, Autor
    visibilidad.value = true; // Mostrar el botón
  } else {
    visibilidad.value = false; // Ocultar el botón
  }
}
//visibilidad del componente para agregar
const toggleAgregarPublicacion = () => {
  mostrarAgregarPublicacion.value = !mostrarAgregarPublicacion.value;
};
</script>

<style>

</style>

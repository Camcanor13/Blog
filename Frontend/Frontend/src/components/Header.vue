<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';


const nombreUsuario = ref('');
const tipoRol = ref('');
const router = useRouter();


// Leer los datos del localStorage al montar el componente
onMounted(() => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    // Parsear los datos JSON a un objeto JavaScript
    const user = JSON.parse(userData);
    console.log(user);
    nombreUsuario.value = user.userName;
    if(user.rol==="1"){
    tipoRol.value="Autor"
  }else if(user.rol==="2"){
     tipoRol.value='Moderador'
  }else{
    tipoRol.value="Lector"
  }
  }

});

const logout = () => {
  // Eliminar los datos del localStorage
  localStorage.removeItem('userData');
  alert("Se cerro sesion exitosamente :)");
  router.push('/');
};
</script>

<template>
  <p>Tu cuenta tiene permisos de:{{ tipoRol }}</p>
  <p>{{ nombreUsuario }}</p>
  <button @click="logout">Cerrar sesión</button>
</template>

<style scoped>
/* Estilos opcionales */
</style>
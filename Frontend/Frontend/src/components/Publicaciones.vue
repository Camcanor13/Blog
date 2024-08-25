<template>
    <div>
      <div v-if="publications.length">
        <h1>Publicaciones:</h1>
        <ul>
          <li v-for="publication in publications" :key="publication.id">
            <h2>Titulo: {{ publication.title }}</h2>
            <h3>Autor: {{ publication.author }}</h3>  
            <h3>Fecha de Publicacion: {{ publication.date }}</h3>
            <h3>Estado: {{ publication.status }}</h3>
            <p>{{ publication.body}}</p>
            <h3>Comentarios:{{ publication.comments }}</h3>
            <h3>Calificacion:{{ publication.qualification }}</h3>
          </li>
        </ul>
      </div>
      <div v-else>
        <p>No hay publicaciones disponibles.</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  
  // Crear una referencia reactiva para almacenar las publicaciones
  const publications = ref([]);
  
  // Función para manejar la obtención de las publicaciones
  const imprimir = async () => {
    try {
      // Enviar una solicitud GET al backend para obtener las publicaciones
      const response = await axios.get('/api/Publications');
      console.log(response.data);
      publications.value = response.data; // Almacenar las publicaciones en la referencia reactiva
    } catch (error) {
      console.error('Error al obtener las publicaciones:', error);
    }
  };
  
  // Llamar a la función imprimir cuando el componente se monta
  onMounted(() => {
    imprimir();
  });
  </script>
  
  <style scoped>
  /* Estilos opcionales */
  div {
    margin: 20px;
  }
  ul {
    list-style-type: none;
    padding: 0;
  }
  li {
    margin: 5px 0;
  }
  </style>
  
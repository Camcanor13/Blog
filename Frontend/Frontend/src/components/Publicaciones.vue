<template>
  <!-- CAMPO PARA BÚSQUEDA -->
   <div class="border border-black">
    <form >
    <h3>Búsqueda rápida</h3>
    <select v-model="filterBy">
      <option value="title">Título</option>
      <option value="date">Fecha</option>
      <option value="author">Autor</option>
    </select>
    <input v-if="filterBy === 'title' || filterBy === 'author'" v-model="searchTerm" placeholder="Buscar..." type="text" />
    <input v-if="filterBy === 'date'" v-model="searchTerm" type="date" />
  </form>

   </div>
  
   
  <!-- CONTENEDOR QUE IMPRIME LAS PUBLICACIONES -->
  <div>
    <div v-if="filteredPublications.length">
      <h1>Publicaciones:</h1>
      <ul >
        <li v-for="publication in filteredPublications" :key="publication.id" class="border-y border-y-black p-5">
          <h2>Título: {{ publication.title }}</h2>
          <h3>Autor: {{ publication.author }}</h3>  
          <h3>Fecha de Publicación: {{ formatDate(publication.date) }}</h3>
          <h3>Estado: {{ publication.status }}</h3>
          <p>{{ publication.body }}</p>
          <h3>Comentarios: {{ publication.comments }}</h3>
          <h3>Calificación: {{ publication.qualification }}</h3>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>No hay publicaciones disponibles.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

const publications = ref([]);
const searchTerm = ref('');
const filterBy = ref('title');
const userRol = ref(null);  // Rol del usuario actual

// Función para manejar la obtención de las publicaciones
const imprimir = async () => {
  try {
    const response = await axios.get('/api/Publications');
    publications.value = response.data; 
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

onMounted(() => {
  imprimir();

  // Obtener el rol del usuario desde el localStorage
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
});

// Filtrar las publicaciones a partir de la búsqueda realizada y el rol del usuario
const filteredPublications = computed(() => {
  return publications.value.filter(publication => {
    const term = searchTerm.value.toLowerCase();
    
    // Filtrado por tipo de búsqueda
    let matchesSearch = false;
    if (filterBy.value === 'title') {
      matchesSearch = publication.title.toLowerCase().includes(term);
    } else if (filterBy.value === 'date') {
      matchesSearch = publication.date.includes(term); // Ya que es fecha, no se usa toLowerCase
    } else if (filterBy.value === 'author') {
      matchesSearch = publication.author.toLowerCase().includes(term);
    }

    // Filtrado por rol: Si el rol es 3, excluir publicaciones bloqueadas
    const isVisible = !((userRol.value === "3" || userRol.value=="1") && publication.status === "bloqueado");

    return matchesSearch && isVisible;
  });
});

// Función para formatear la fecha
const formatDate = (dateString) => {
  const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
  return new Date(dateString).toLocaleDateString(undefined, options);
};
</script>

<style scoped>
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
form {
  margin-bottom: 20px;
}
input {
  margin-left: 10px;
}
</style>

<template>
  <div class="max-w-4xl mx-auto px-10 py-6 bg-white rounded-lg shadow-md">
    <!-- CAMPO PARA BÚSQUEDA -->
    <div class="border-b border-gray-200 mb-6">
      <form class="flex flex-wrap items-center justify-between p-4">
        <h3 class="flex-1 font-bold">Búsqueda rápida:</h3>
        <select v-model="filterBy" class="mx-2 p-2 border border-gray-300 rounded">
          <option value="title">Título</option>
          <option value="date">Fecha</option>
          <option value="author">Autor</option>
        </select>
        <input v-if="filterBy === 'title' || filterBy === 'author'" v-model="searchTerm" placeholder="Buscar..." type="text" class="flex-1 p-2 border border-gray-300 rounded" />
        <input v-if="filterBy === 'date'" v-model="searchTerm" type="date" class="p-2 border border-gray-300 rounded" />
      </form>
    </div>
    
    <!-- CONTENEDOR QUE IMPRIME LAS PUBLICACIONES -->
    <div v-if="filteredPublications.length">
      <h1 class="text-2xl font-bold leading-tight md:text-3xl mb-6">Publicaciones:</h1>
      <ul class="space-y-5">
        <li v-for="publication in filteredPublications" :key="publication.id" class="border border-gray-200 rounded-lg p-5 hover:shadow-lg">
          <div class="flex justify-between items-center">
            <span class="font-light text-gray-600">{{ formatDate(publication.date) }}</span>
            <span class="px-2 py-1 bg-gray-600 text-white font-bold rounded hover:bg-gray-500">{{ publication.status }}</span>
          </div>
          <div class="mt-2">
            <a class="text-2xl text-gray-700 font-bold hover:underline" href="#">{{ publication.title }}</a>
            <p class="mt-2 text-gray-600">{{ publication.body }}</p>
          </div>
          <div class="flex justify-between items-center mt-4">
            <span class="text-blue-500 hover:underline">Leer más</span>
            <div>
              <a class="flex items-center" href="#">
                <h1 class="text-gray-700 font-bold hover:underline">{{ publication.author }}</h1>
              </a>
            </div>
          </div>
        </li>
      </ul>
    </div>
    <div v-else>
      <p class="text-center text-gray-500">No hay publicaciones disponibles.</p>
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
    
    let matchesSearch = false;
    if (filterBy.value === 'title') {
      matchesSearch = publication.title.toLowerCase().includes(term);
    } else if (filterBy.value === 'date') {
      matchesSearch = publication.date.includes(term); 
    } else if (filterBy.value === 'author') {
      matchesSearch = publication.author.toLowerCase().includes(term);
    }

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
ul {
  list-style-type: none;
  padding: 0;
}
</style>


<!-- <template>
  <div class="max-w-4xl mx-auto px-10 py-6 bg-white rounded-lg shadow-md">
    CAMPO PARA BÚSQUEDA 
    <div class="border-b border-gray-200 mb-6">
      <form class="flex flex-wrap items-center justify-between p-4">
        <h3 class="flex-1 font-bold">Búsqueda rápida:</h3>
        <select v-model="filterBy" class="mx-2 p-2 border border-gray-300 rounded">
          <option value="title">Título</option>
          <option value="date">Fecha</option>
          <option value="author">Autor</option>
        </select>
        <input v-if="filterBy === 'title' || filterBy === 'author'" v-model="searchTerm" placeholder="Buscar..." type="text" class="flex-1 p-2 border border-gray-300 rounded" />
        <input v-if="filterBy === 'date'" v-model="searchTerm" type="date" class="p-2 border border-gray-300 rounded" />
      </form>
    </div>
    
    CONTENEDOR QUE IMPRIME LAS PUBLICACIONES 
    <div v-if="filteredPublications.length">
      <h1 class="text-2xl font-bold leading-tight md:text-3xl mb-6">Publicaciones:</h1>
      <ul class="space-y-5">
        <li v-for="publication in filteredPublications" :key="publication.id" class="border border-gray-200 rounded-lg p-5 hover:shadow-lg">
          <div class="flex justify-between items-center">
            <span class="font-light text-gray-600">{{ formatDate(publication.date) }}</span>
            <span class="px-2 py-1 bg-gray-600 text-white font-bold rounded hover:bg-gray-500">{{ publication.status }}</span>
          </div>
          <div class="mt-2">
            <a class="text-xl text-gray-700 font-bold hover:underline" href="#">{{ publication.title }}</a>
            <p class="mt-2 text-gray-600">{{ publication.body }}</p>
          </div>
          <div class="flex justify-between items-center mt-4">
            <span class="text-blue-500 hover:underline">Leer más</span>
            <div>
              <a class="flex items-center" href="#">
                <h1 class="text-gray-700 font-bold hover:underline">{{ publication.author }}</h1>
              </a>
            </div>
          </div>
        </li>
      </ul>
    </div>
    <div v-else>
      <p class="text-center text-gray-500">No hay publicaciones disponibles.</p>
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
    
    let matchesSearch = false;
    if (filterBy.value === 'title') {
      matchesSearch = publication.title.toLowerCase().includes(term);
    } else if (filterBy.value === 'date') {
      matchesSearch = publication.date.includes(term); 
    } else if (filterBy.value === 'author') {
      matchesSearch = publication.author.toLowerCase().includes(term);
    }

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
ul {
  list-style-type: none;
  padding: 0;
}
</style>
-->

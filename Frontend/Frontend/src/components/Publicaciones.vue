<template>
  <div class="max-w-6xl mx-auto px-12 py-10 bg-white rounded-lg shadow-md">
    <!-- CAMPO PARA BÚSQUEDA -->
    <div class="mb-10">
      <form class="flex items-center justify-between bg-gray-100 p-4 rounded-lg">
        <h3 class="text-xl font-semibold text-gray-700">Búsqueda rápida:</h3>
        <div class="flex flex-grow items-center ml-4">
          <select v-model="filterBy" class="p-2 mr-4 bg-white border border-gray-300 rounded-md text-gray-700">
            <option value="title">Título</option>
            <option value="date">Fecha</option>
            <option value="author">Autor</option>
          </select>
          <input v-if="filterBy === 'title' || filterBy === 'author'" v-model="searchTerm" placeholder="Buscar..."
            type="text" class="flex-grow p-2 bg-white border border-gray-300 rounded-md text-gray-700" />
          <input v-if="filterBy === 'date'" v-model="searchTerm" type="date"
            class="p-2 bg-white border border-gray-300 rounded-md text-gray-700" />
        </div>
      </form>
    </div>

    <!-- CONTENEDOR DE PUBLICACIONES -->
    <div v-if="filteredPublications.length">
<<<<<<< HEAD
      <h1 class="text-2xl font-bold leading-tight md:text-3xl mb-6">Publicaciones:</h1>
      <ul class="space-y-5">
        <li v-for="publication in filteredPublications" :key="publication.id" class="border border-gray-200 rounded-lg p-5 hover:shadow-lg" @click="selectPublication(publication.id)">
=======
      <h1 class="text-4xl font-bold text-gray-800 mb-8">Publicaciones:</h1>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div v-for="publication in filteredPublications" :key="publication.id"
          class="bg-white border border-gray-200 rounded-lg p-6 hover:shadow-lg transition-shadow duration-300">
>>>>>>> 131601e96123c19294b1258848fbc46a1bfe9215
          <div class="flex justify-between items-center">
            <span class="text-sm font-light text-gray-500">{{ formatDate(publication.date) }}</span>
            <span :class="statusClass(publication.status)" class="text-sm font-bold">{{ publication.status }}</span>
          </div>
          <a class="mt-2 block text-2xl text-gray-800 font-semibold hover:text-gray-600" href="#">{{ publication.title
            }}</a>
          <p class="mt-1 text-gray-600">{{ publication.body }}</p>
          <div class="flex justify-between items-center mt-4">
<<<<<<< HEAD
            <!-- <span class="text-blue-500 hover:underline">Leer más</span> -->
             <div  v-if="selectedPublicationId === publication.id">
              <div v-for="coment in comments" :key="coment.id">
                <h2>Comentarios:</h2>
              
                  <p>{{coment.comment}} - comentario realizado por: {{ coment.userName }}</p>
              </div>
             <p>Comentar:</p>
             <input type="text" placeholder="Deja un comentario" value="comentario">
             <button>Enviar</button>

             </div>
             
            <div>
              <a class="flex items-center" href="#">
                <h1 class="text-gray-700 font-bold hover:underline">{{ publication.author }}</h1>
              </a>
            </div>
=======
            <a class="text-blue-600 hover:text-blue-800">Leer más</a>
            <a class="text-gray-800 hover:text-gray-600">{{ publication.author }}</a>
>>>>>>> 131601e96123c19294b1258848fbc46a1bfe9215
          </div>
        </div>
      </div>
    </div>
    <div v-else>
      <p class="text-center text-gray-500">No hay publicaciones disponibles.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';


const comments = ref([]);
const publications = ref([]);
const searchTerm = ref('');
const filterBy = ref('title');
<<<<<<< HEAD
const userRol = ref(null);  // Rol del usuario actual
const selectedPublicationId = ref(null);

// Función para manejar la obtención de las publicaciones
const imprimir = async () => {
  try {
    const response = await axios.get('/api/Publications');
    publications.value = response.data; 
    console.log(publications);
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

const selectPublication = (id) => {
  selectedPublicationId.value = id;
  imprimirComentarios(id);
};

const imprimirComentarios = async (idpublicacion) => {
  try {
    const response = await axios.get(`/api/ListComment?idpublicacion=${idpublicacion}`);
    comments.value = response.data; 
    console.log(comments);
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

onMounted(() => {
  imprimir();


  // Obtener el rol del usuario desde el localStorage
=======
const userRol = ref(null);

onMounted(() => {
  imprimir();
>>>>>>> 131601e96123c19294b1258848fbc46a1bfe9215
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
});

const imprimir = async () => {
  try {
    const response = await axios.get('/api/Publications');
    publications.value = response.data;
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

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
    const isVisible = !((userRol.value === "3" || userRol.value == "1") && publication.status === "bloqueado");
    return matchesSearch && isVisible;
  });
});

const statusClass = (status) => {
  return `px-3 py-1 bg-gray-500 text-white font-bold rounded-none`;
};

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
<<<<<<< HEAD

=======
>>>>>>> 131601e96123c19294b1258848fbc46a1bfe9215

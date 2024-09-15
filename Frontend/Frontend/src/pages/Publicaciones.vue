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
      <h1 class="text-2xl font-bold leading-tight md:text-3xl mb-6">Publicaciones:</h1>
      <ul class="space-y-5">
        <li v-for="publication in filteredPublications" :key="publication.id" class="border border-gray-200 rounded-lg p-5 hover:shadow-lg" @click="selectPublication(publication.id)">
          <div class="flex justify-between items-center">
            <span class="text-sm font-light text-gray-500">{{ formatDate(publication.date) }}</span>
            <span :class="statusClass(publication.status)" class="text-sm font-bold">{{ publication.status }}</span>
            <!-- En el template -->
<button v-if="isUserModerator" @click.stop="changePublicationStatus(publication.id)" class="bg-yellow-500 text-white px-4 py-2 rounded-md">CAMBIAR ESTADO</button>

          </div>
          <a class="mt-2 block text-2xl text-gray-800 font-semibold hover:text-gray-600" href="#">{{ publication.title }}</a>
          <p class="mt-1 text-gray-600">{{ publication.body }}</p>
          <div class="flex justify-between items-center mt-4">
            <div v-if="selectedPublicationId === publication.id">
              <div v-for="coment in comments" :key="coment.id">
                <h2>Comentarios:</h2>
                <p>{{ coment.comment }} - comentario realizado por: {{ coment.userName }}</p>
              </div>
              <p>Comentar:</p>
              <input v-model="comentario" type="text" placeholder="Deja un comentario" id="comentario" name="comentario" class="p-2 border border-gray-300 rounded-md" />
              <button @click="submitComment(publication.id)" type="submit" class="mt-2 bg-blue-500 text-white px-4 py-2 rounded-md">Enviar</button>
            </div>
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
import Swal from 'sweetalert2';
const comments = ref([]);
const publications = ref([]);
const searchTerm = ref('');
const filterBy = ref('title');
const userRol = ref(null);  // Rol del usuario actual
const selectedPublicationId = ref(null);
const idUsuario = ref('');  // Asegúrate de asignar el ID del usuario
const comentario = ref('');  // Hacer el campo reactivo

// Obtener rol del usuario actual
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;  // Asignar rol del usuario
    idUsuario.value = user.id; // Asegúrate de asignar el ID del usuario
  }
};

const isUserModerator = computed(() => userRol.value === "2");

const changePublicationStatus = async (idpublicacion) => {
  try {
    const { value: newStatus } = await Swal.fire({
      title: 'Seleccionar nuevo estado',
      input: 'select',
      inputOptions: {
        'aceptado': 'Aceptado',
        'revision': 'Revisión',
        'bloqueado': 'Bloqueado'
      },
      inputPlaceholder: 'Selecciona un estado',
      showCancelButton: true,
      confirmButtonText: 'Actualizar',
      cancelButtonText: 'Cancelar',
      inputValidator: (value) => {
        if (!value) {
          return 'Debes seleccionar un estado';
        }
      }
    });

    if (newStatus) {
      const response = await axios.put(`/api/Status/UpdateStatus/${idpublicacion}`, {
        newStatus: newStatus
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (response.status === 200) {
        Swal.fire('Éxito', 'Estado actualizado', 'success');
        await imprimir(); // Volver a cargar las publicaciones actualizadas
      }
    }
  } catch (error) {
    console.error('Error al actualizar la publicación:', error.response ? error.response.data : error.message);
    Swal.fire('Error', 'Error al actualizar la publicación', 'error');
  }
};


// Seleccionar publicación y obtener los comentarios
const selectPublication = (id) => {
  selectedPublicationId.value = id;
  imprimirComentarios(id);
};

// Obtener comentarios de una publicación específica
const imprimirComentarios = async (idpublicacion) => {
  try {
    const response = await axios.get(`/api/ListComment?idpublicacion=${idpublicacion}`);
    comments.value = response.data;
  } catch (error) {
    console.error('Error al obtener los comentarios:', error);
  }
};

// Enviar comentario
const submitComment = async (idpublicacion) => {
  getUserRole();  // Actualizar el rol del usuario

  if (comentario.value.trim() === '') {
    alert('El campo de comentario está vacío.');
    return; // No continuar si el comentario está vacío
  }

  try {
    console.log('Enviando comentario:', {
      id_publicacion: idpublicacion,
      id_usuario: idUsuario.value,
      comentario: comentario.value
    });

    const response = await axios.post(`/api/AddComment`, {
      id_publicacion: idpublicacion,
      id_usuario: idUsuario.value,
      comentario: comentario.value
    });

    console.log('Comentario enviado:', response.data);
    comentario.value = '';  // Limpiar el campo después de enviar
    await imprimirComentarios(idpublicacion);  // Actualizar comentarios
  } catch (error) {
    console.error('Error al enviar el comentario:', error.response ? error.response.data : error.message);
  }
};

// Obtener publicaciones
onMounted(() => {
  getUserRole();  // Actualizar el rol del usuario al montar el componente
  imprimir();
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

    // Verificar el filtro de búsqueda
    if (filterBy.value === 'title') {
      matchesSearch = publication.title.toLowerCase().includes(term);
    } else if (filterBy.value === 'date') {
      matchesSearch = publication.date.includes(term);
    } else if (filterBy.value === 'author') {
      matchesSearch = publication.author.toLowerCase().includes(term);
    }

    // Verificar visibilidad según el rol del usuario
    const isVisible = !(userRol.value === "1" || userRol.value === "3") || publication.status !== "bloqueado" && publication.status!=="revision";

    return matchesSearch && isVisible;
  });
});

// Asignar clases de estado a las publicaciones
const statusClass = (status) => {
  return `px-3 py-1 ${status === 'bloqueado' ? 'bg-red-500' : 'bg-gray-500'} text-white font-bold rounded-none`;
};

// Formatear la fecha
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

<template>
  <div class="max-w-6xl mx-auto px-12 py-10 rounded-lg ">
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
        <li v-for="publication in filteredPublications" :key="publication.id"
          class="border border-gray-200 p-5 bg-white rounded-lg shadow-md hover:shadow-lg hover:shadow-gray-500 hover:bg-slate-300 cursor-pointer transition-shadow duration-300 p-6 "
          @click="selectPublication(publication.id)">
          <div class="flex justify-between items-center">
            <span class="text-sm font-light text-gray-500">{{ formatDate(publication.date) }}</span>
            <span :class="statusClass(publication.status)" class="text-sm font-bold">{{ publication.status }}</span>
            <!-- En el template -->
            <button v-if="isUserModerator" @click.stop="changePublicationStatus(publication.id)"
              class="px-4 py-2 bg-cyan-400 text-dark font-bold  rounded-md hover:bg-yellow-600 hover:text-white ">CAMBIAR
              ESTADO</button>

          </div>
          <a class="mt-2 block text-2xl text-gray-800 font-semibold hover:text-gray-600" href="#">{{ publication.title
            }}</a>
          <p class="mt-1 text-gray-600">{{ publication.body }}</p>
          <div class="flex justify-between items-center mt-4">
            <div v-if="selectedPublicationId === publication.id">
              <h2>Comentarios:</h2>
              <div v-for="coment in comments" :key="coment.id">
           
                <p>{{coment.id}}{{ coment.comment }} - comentario realizado por: {{ coment.userName }}</p>
              </div>
             
              <input v-model="comentario" type="text" placeholder="Deja un comentario" id="comentario" name="comentario" class="p-2 border border-gray-300 rounded-md" />
              <button @click="submitComment(publication.id)" type="submit" class="mt-2 bg-blue-500 text-white px-4 py-2 rounded-md">Enviar</button>

              <p>Calificaciones:</p>
              <div v-if="califications.length === 0">
                <p>No hay calificaciones disponibles</p>
              </div>
            <div v-for="calificacion in califications" :key="calificacion.id">
              <p>{{ calificacion.calification }} estrella, realizado por: {{ calificacion.userName }}</p>
              <button v-if="isOwner(calificacion.userName)" @click="deleteCalification(calificacion.id)">Eliminar</button>
            </div>
              <div class="border">
              <button v-if="isUserLector" @click="submitCalification(publication.id)" type="submit" class="border p-3 bg-green-500 text-white rounded-md">Calificar</button>
             
              <select v-if="isUserLector" v-model="selectedCalification">
                <option value="0">0 estrella (Muy Malo)</option> 
               <option value="1">1 estrella (Malo)</option> 
               <option value="2">2 estrella(Regular)</option> 
               <option value="3">3 estrella (Normal)</option> 
               <option value="4">4 estrella(Bueno)</option> 
               <option value="5">5 estrella (Excelente)</option> 
              </select>
            </div>
            <button  v-if="isUserLector" @click="toggleLike(publication.id)" class="bg-purple-600 text-white px-4 py-2 rounded-md">
              {{ userHasLiked ? 'No me gusta' : 'Me gusta' }}
            </button>
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
const idUsuario = ref('');  
const usuario = ref(''); // Asegúrate de asignar el ID del usuario
const comentario = ref('');  // Hacer el campo reactivo
const selectedCalification = ref(null);
const califications = ref([]);


// Obtener rol del usuario actual
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;  // Asignar rol del usuario
    idUsuario.value = user.id;
    usuario.value=user.userName;
    console.log(usuario.value);
     // Asegúrate de asignar el ID del usuario
  }
};

const isOwner = (userName) => {
  return userName === usuario.value;
};
const isUserModerator = computed(() => userRol.value === "2");
const isUserLector = computed(() => userRol.value === "3");
const isvisibibleBoton = computed(() => {
  return calificacion.value.some(calif => calif.userName === usuario.value);
});



// Cambiar de estado
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
  comments.value = []; // Limpiar comentarios
  califications.value = []; // Limpiar calificaciones
  imprimirComentarios(id);
  imprimirCalifications(id);
 getLikeStatus(id); // Verificar "Me gusta" para la publicación seleccionada
};

const imprimirComentarios = async (idpublicacion) => {
  try {
    const response = await axios.get(`/api/ListComment?idpublicacion=${idpublicacion}`);
    comments.value = response.data;
  } catch (error) {
    console.error('Error al obtener los comentarios:', error);
  }
};

// Obtener calificaciones de una publicación específica
const imprimirCalifications = async (idPublication) => {
  try {
    const response = await axios.get(`/api/ListCalification/${idPublication}`);
    console.log('Calificaciones:', response.data);
    califications.value = response.data;
  } catch (error) {
    console.error('Error al obtener las calificaciones:', error);
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



const imprimir = async () => {
  try {
    const response = await axios.get('/api/Publications');
    publications.value = response.data;
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

// Obtener publicaciones
onMounted(() => {
  getUserRole();  // Actualizar el rol del usuario al montar el componente
  imprimir();
 
  
});

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
    const isVisible = !(userRol.value === "1" || userRol.value === "3") || publication.status !== "bloqueado" && publication.status !== "revision";

    return matchesSearch && isVisible;
  });
});

// Asignar clases de estado a las publicaciones
const statusClass = (status) => {
  return `px-3 py-1 ${status === 'bloqueado' ? 'inline-flex items-center rounded-md bg-red-50 px-2 py-1 text-xs font-medium text-red-700 ring-1 ring-inset ring-red-600/10' : 'inline-flex items-center rounded-md bg-gray-100 px-2 py-1 text-xs font-medium text-gray-600 ring-1 ring-inset ring-gray-500/10'} font-bold rounded italic text-base`;
};

// Formatear la fecha
const formatDate = (dateString) => {
  const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
  return new Date(dateString).toLocaleDateString(undefined, options);
};

// Enviar calificación
const submitCalification = async (idpublicacion) => {
  getUserRole();

  if (!selectedCalification.value) {
    alert('Por favor selecciona una calificación.');
    return;
  }

  try {
    const response = await axios.post(`/api/AddCalification`, {
      id_publicacion: idpublicacion,
      id_usuario: idUsuario.value,
      calificacion: selectedCalification.value
    });

    if (response.status === 200) {
      Swal.fire('Calificación enviada', 'Gracias por tu valoración', 'success');
      imprimirCalifications(idpublicacion)
    }
  } catch (error) {
    console.error('Error al enviar la calificación:', error.response ? error.response.data : error.message);
    Swal.fire('Error', `${error.response.data}`, 'error');
  }
};

const deleteCalification = async (calificationId) => {
  try {
    const response = await axios.delete(`/api/DeleteCalification/${calificationId}`);
    
    if (response.status === 200) {
      Swal.fire('Éxito', 'Calificación eliminada', 'success');
      // Actualiza la lista de calificaciones después de la eliminación
      await imprimirCalifications(selectedPublicationId.value);
    }
  } catch (error) {
    console.error('Error al eliminar la calificación:', error.response ? error.response.data : error.message);
    Swal.fire('Error', 'Error al eliminar la calificación', 'error');
  }
};


const userHasLiked = ref(false);

const toggleLike = async (publicationId) => {
  if (userHasLiked.value) {
    await dislikePublication(publicationId);
  } else {
    await likePublication(publicationId);
  }
};

const likePublication = async (publicationId) => {
  try {
    const response = await axios.post('/api/MeGusta', {
      publicationId: publicationId,
      userId: idUsuario.value,
    });
    if (response.status === 200) {
      Swal.fire('Éxito', 'Has dado "Me gusta" a la publicación', 'success');
      userHasLiked.value = true;
    }
  } catch (error) {
    console.error('Error al dar "Me gusta":', error.response ? error.response.data : error.message);
    Swal.fire('Error', 'Error al dar "Me gusta"', 'error');
  }
};

const dislikePublication = async (publicationId) => {
  try {
    const response = await axios.delete(`/api/MeGusta/${publicationId}/${idUsuario.value}`);
    if (response.status === 200) {
      Swal.fire('Éxito', 'Has eliminado tu "Me gusta" de la publicación', 'success');
      userHasLiked.value = false;
    }
  } catch (error) {
    console.error('Error al eliminar "Me gusta":', error.response ? error.response.data : error.message);
    Swal.fire('Error', 'Error al eliminar "Me gusta"', 'error');
  }
};
const getLikeStatus = async (publicationId) => {
  try {
    const response = await axios.get(`/api/MeGusta/publicacion/${publicationId}`);
    const usersWhoLiked = response.data;

    console.log('Usuarios que han dado me gusta:', usersWhoLiked);
    console.log('Usuario actual:', usuario.value);

    // Verifica si el nombre de usuario actual está en la lista
    userHasLiked.value = usersWhoLiked.some(user => user.userName === usuario.value);
    console.log('Estado de Me gusta:', userHasLiked.value);
  } catch (error) {
    console.error('Error al obtener "Me gusta":', error.response ? error.response.data : error.message);
  }
};




</script>


<style scoped>
ul {
  list-style-type: none;
  padding: 0;
}
</style>

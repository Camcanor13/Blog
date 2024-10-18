<template>

  <div class="w-full flex justify-center overflow-hidden">
    <div class="grid grid-cols-[25%,55%,20%] w-11/12 md:gap-x-4">
      <!-- CAMPO PARA BÚSQUEDA -->
      <div class=" w-full flex flex-wrap gap-y-6 py-5 h-fit">
        <section class="bg-gray-100 shadow-slate-100 shadow-md border-gray-100 p-5 h-fit w-full rounded-md">
          <form class="flex flex-wrap items-center justify-between w-full">
            <h3 class="text-xl font-semibold text-gray-700 w-full">Búsqueda rápida:</h3>
            <div class="flex flex-grow items-center">
              <select v-model="filterBy"
                class="p-2  mr-4 bg-white border border-gray-300 rounded-md text-gray-700 w-1/4">
                <option value="title">Título</option>
                <option value="date">Fecha</option>
                <option value="author">Autor</option>
              </select>
              <input v-if="filterBy === 'title' || filterBy === 'author'" v-model="searchTerm" placeholder="Buscar..."
                type="text" class="flex-grow p-2 bg-white border border-gray-300 rounded-md text-gray-700 w-3/4" />
              <input v-if="filterBy === 'date'" v-model="searchTerm" type="date"
                class="p-2 bg-white border border-gray-300 rounded-md text-gray-700 w-3/4" />
            </div>
          </form>
        </section>
        <div>
          <h3 class="font-bold">Autores</h3>
          <section class="bg-gray-100 shadow-slate-100 shadow-md border-gray-900 p-5 rounded-md flex flex-wrap gap-2">
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Ciencia ficción</p>
            </article>
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Fantasía</p>
            </article>
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Terror</p>
            </article>
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Novelas</p>
            </article>
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Drama</p>
            </article>
            <article class="border rounded-sm border-black text-center capitalize px-3 py-1">
              <p class="p-0 m-0 text-sm">Informativo</p>
            </article>
          </section>
        </div>

      </div>
      <main class="w-full bg-white border-gray-900 p-5">
        <!-- CONTENEDOR DE PUBLICACIONES -->
        <div v-if="filteredPublications.length">
          <!-- <h1 class="text-2xl font-bold leading-tight md:text-3xl mb-6">Publicaciones:</h1> -->
          <div class="flex flex-wrap gap-y-5">
            <article v-for="publication in filteredPublications" :key="publication.id"
              class="w-full border border-gray-200 p-5 bg-white rounded-lg shadow-md hover:shadow-lg hover:shadow-gray-500 hover:bg-slate-100 cursor-pointer transition-shadow duration-300 p-6">
              <div class="flex flex-wrap justify-between mb-4">
                <div class="w-1/5 flex flex-wrap items-center">
                  <div class="w-1/4">
                    <button id="headlessui-menu-button-3" type="button" aria-haspopup="menu" aria-expanded="false"
                      data-headlessui-state=""
                      class="relative flex rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 hover:bg-white"><span
                        class="absolute -inset-1.5"></span><span class="sr-only">Open user menu</span><img
                        class="h-8 w-8 rounded-full"
                        src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&amp;ixid=eyJhcHBfaWQiOjEyMDd9&amp;auto=format&amp;fit=facearea&amp;facepad=2&amp;w=256&amp;h=256&amp;q=80"
                        alt=""></button>
                  </div>
                  <div class="w-3/4 ">
                    <p class="m-0 px-4 text-gray-700 font-bold text-2xl">{{ publication.author }}</p>
                    <small class="m-0 px-4 font-light text-gray-500">{{ formatDate(publication.date) }}</small>
                  </div>
                </div>
                <div class="w-1/2 flex justify-end">
                  <span :class="statusColor[publication.status]" data-v-bb81842d=""
                    class="px-3 py-1 inline-flex items-center rounded-md px-2 py-1 text-xs font-medium text-gray-600 ring-1 ring-inset ring-gray-500/10 font-bold rounded italic text-base text-sm font-bold capitalize h-fit"
                    @click="changePublicationStatus(publication.id)">{{
                      publication.status
                    }}</span>
                </div>
              </div>
              <h2 class="mt-2 block text-2xl text-gray-800 font-semibold hover:text-gray-600 my-4"><b>{{
                publication.title
                  }}</b></h2>
              <!--<pre class="p-4 text-justify">{{ publication.body }}</pre>-->
              <p class="mt-1 text-gray-600 py-5">{{ publication.body }}</p>
              <div class="grid grid-cols-3 gap-3 items-center justify-items-center border-y border-black">
                <button class="w-full hover:bg-gray-100  px-3 py-1" v-if="isUserLector"
                  @click="toggleLike(publication.id)">{{ userHasLiked ? 'No me gusta' : 'Me gusta' }}</button>
                <button class="w-full hover:bg-gray-100  px-3 py-1"
                  @click="openComentarios(publication.id)">Comentar</button>
                <button class="w-full hover:bg-gray-100  px-3 py-1"
                  @click="openCalificaciones(publication.id)">Calificar</button>
              </div>
              <div>
                <Comentarios :idpublicacion="publication.id" :idUsuario="idUsuario"
                  v-if="comentarioIsOpen.includes(publication.id)" />
                <Calificaciones :idpublicacion="publication.id" :idUsuario="idUsuario" :userName="usuario" :userType="isUserLector"
                  v-if="calificacionIsOpen.includes(publication.id)" />
              </div>
            </article>
          </div>
<!--
          <ul class="space-y-5">
            <li v-for="publication in filteredPublications" :key="publication.id"
              class="border border-gray-200 p-5 bg-white rounded-lg shadow-md hover:shadow-lg hover:shadow-gray-500 hover:bg-slate-300 cursor-pointer transition-shadow duration-300 p-6 "
              @click="selectPublication(publication.id)">
              <div class="flex justify-between items-center">
                <span class="text-sm font-light text-gray-500">{{ formatDate(publication.date) }}</span>
                <span :class="statusColor[publication.status]" class="text-sm font-bold capitalize"
                  @click="changePublicationStatus(publication.id)">{{
                    publication.status
                  }}</span>

              </div>
              <a class="mt-2 block text-2xl text-gray-800 font-semibold hover:text-gray-600" href="#">{{
                publication.title
              }}</a>
              <p class="mt-1 text-gray-600">{{ publication.body }}</p>
              <div class="flex justify-between items-center mt-4">
                <div v-if="selectedPublicationId === publication.id">
                  <h2>Comentarios:</h2>
                  <div v-for="coment in comments" :key="coment.id">

                    <p>{{ coment.id }}{{ coment.comment }} - comentario realizado por: {{ coment.userName }}</p>
                  </div>

                  <input v-model="comentario" type="text" placeholder="Deja un comentario" id="comentario"
                    name="comentario" class="p-2 border border-gray-300 rounded-md" />
                  <button @click="submitComment(publication.id)" type="submit"
                    class="mt-2 bg-blue-500 text-white px-4 py-2 rounded-md">Enviar</button>

                  <p>Calificaciones:</p>
                  <div v-if="califications.length === 0">
                    <p>No hay calificaciones disponibles</p>
                  </div>
                  <div v-for="calificacion in califications" :key="calificacion.id">
                    <p>{{ calificacion.calification }} estrella, realizado por: {{ calificacion.userName }}</p>
                    <button v-if="isOwner(calificacion.userName)"
                      @click="deleteCalification(calificacion.id)">Eliminar</button>
                  </div>
                  <div class="border">
                    <button v-if="isUserLector" @click="submitCalification(publication.id)" type="submit"
                      class="border p-3 bg-green-500 text-white rounded-md">Calificar</button>

                    <select v-if="isUserLector" v-model="selectedCalification">
                      <option value="0">0 estrella (Muy Malo)</option>
                      <option value="1">1 estrella (Malo)</option>
                      <option value="2">2 estrella(Regular)</option>
                      <option value="3">3 estrella (Normal)</option>
                      <option value="4">4 estrella(Bueno)</option>
                      <option value="5">5 estrella (Excelente)</option>
                    </select>
                  </div>
                  <button v-if="isUserLector" @click="toggleLike(publication.id)"
                    class="bg-purple-600 text-white px-4 py-2 rounded-md">
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
        -->
        </div>

        <div v-else>
          <p class="text-center text-gray-500">No hay publicaciones disponibles.</p>
        </div>
      </main>
      <div class=" w-full flex-wrap gap-y-6 py-5 h-fit gap-4">
        <div>
          <h3 class="font-bold">Social plugins</h3>
          <section
            class="bg-gray-100 shadow-slate-100 shadow-md border-gray-900 rounded-md  p-5 grid sm:grid-cols-4 grid-cols-2 gap-2 xl:grid-cols-6">
            <div class="border border-black p-2 rounded-md">
              <a href="" class=""><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                  <path
                    d="M64 32C28.7 32 0 60.7 0 96V416c0 35.3 28.7 64 64 64h98.2V334.2H109.4V256h52.8V222.3c0-87.1 39.4-127.5 125-127.5c16.2 0 44.2 3.2 55.7 6.4V172c-6-.6-16.5-1-29.6-1c-42 0-58.2 15.9-58.2 57.2V256h83.6l-14.4 78.2H255V480H384c35.3 0 64-28.7 64-64V96c0-35.3-28.7-64-64-64H64z" />
                </svg></a>
            </div>
            <div class="border border-black p-2 rounded-md">
              <a href="" class=""><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                  <path
                    d="M194.4 211.7a53.3 53.3 0 1 0 59.3 88.7 53.3 53.3 0 1 0 -59.3-88.7zm142.3-68.4c-5.2-5.2-11.5-9.3-18.4-12c-18.1-7.1-57.6-6.8-83.1-6.5c-4.1 0-7.9 .1-11.2 .1c-3.3 0-7.2 0-11.4-.1c-25.5-.3-64.8-.7-82.9 6.5c-6.9 2.7-13.1 6.8-18.4 12s-9.3 11.5-12 18.4c-7.1 18.1-6.7 57.7-6.5 83.2c0 4.1 .1 7.9 .1 11.1s0 7-.1 11.1c-.2 25.5-.6 65.1 6.5 83.2c2.7 6.9 6.8 13.1 12 18.4s11.5 9.3 18.4 12c18.1 7.1 57.6 6.8 83.1 6.5c4.1 0 7.9-.1 11.2-.1c3.3 0 7.2 0 11.4 .1c25.5 .3 64.8 .7 82.9-6.5c6.9-2.7 13.1-6.8 18.4-12s9.3-11.5 12-18.4c7.2-18 6.8-57.4 6.5-83c0-4.2-.1-8.1-.1-11.4s0-7.1 .1-11.4c.3-25.5 .7-64.9-6.5-83l0 0c-2.7-6.9-6.8-13.1-12-18.4zm-67.1 44.5A82 82 0 1 1 178.4 324.2a82 82 0 1 1 91.1-136.4zm29.2-1.3c-3.1-2.1-5.6-5.1-7.1-8.6s-1.8-7.3-1.1-11.1s2.6-7.1 5.2-9.8s6.1-4.5 9.8-5.2s7.6-.4 11.1 1.1s6.5 3.9 8.6 7s3.2 6.8 3.2 10.6c0 2.5-.5 5-1.4 7.3s-2.4 4.4-4.1 6.2s-3.9 3.2-6.2 4.2s-4.8 1.5-7.3 1.5l0 0c-3.8 0-7.5-1.1-10.6-3.2zM448 96c0-35.3-28.7-64-64-64H64C28.7 32 0 60.7 0 96V416c0 35.3 28.7 64 64 64H384c35.3 0 64-28.7 64-64V96zM357 389c-18.7 18.7-41.4 24.6-67 25.9c-26.4 1.5-105.6 1.5-132 0c-25.6-1.3-48.3-7.2-67-25.9s-24.6-41.4-25.8-67c-1.5-26.4-1.5-105.6 0-132c1.3-25.6 7.1-48.3 25.8-67s41.5-24.6 67-25.8c26.4-1.5 105.6-1.5 132 0c25.6 1.3 48.3 7.1 67 25.8s24.6 41.4 25.8 67c1.5 26.3 1.5 105.4 0 131.9c-1.3 25.6-7.1 48.3-25.8 67z" />
                </svg></a>
            </div>
            <div class="border border-black p-2 rounded-md">
              <a href="" class=""><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                  <path
                    d="M384 32H64C28.7 32 0 60.7 0 96V416c0 35.3 28.7 64 64 64h72.6l-2.2-.8c-5.4-48.1-3.1-57.5 15.7-134.7c3.9-16 8.5-35 13.9-57.9c0 0-7.3-14.8-7.3-36.5c0-70.7 75.5-78 75.5-25c0 13.5-5.4 31.1-11.2 49.8c-3.3 10.6-6.6 21.5-9.1 32c-5.7 24.5 12.3 44.4 36.4 44.4c43.7 0 77.2-46 77.2-112.4c0-58.8-42.3-99.9-102.6-99.9C153 139 112 191.4 112 245.6c0 21.1 8.2 43.7 18.3 56c2 2.4 2.3 4.5 1.7 7c-1.1 4.7-3.1 12.9-4.7 19.2c-1 4-1.8 7.3-2.1 8.6c-1.1 4.5-3.5 5.5-8.2 3.3c-30.6-14.3-49.8-59.1-49.8-95.1C67.2 167.1 123.4 96 229.4 96c85.2 0 151.4 60.7 151.4 141.8c0 84.6-53.3 152.7-127.4 152.7c-24.9 0-48.3-12.9-56.3-28.2c0 0-12.3 46.9-15.3 58.4c-5 19.3-17.6 42.9-27.4 59.3H384c35.3 0 64-28.7 64-64V96c0-35.3-28.7-64-64-64z" />
                </svg></a>
            </div>
            <div class="border border-black p-2 rounded-md">
              <a href="" class=""><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
                  <path
                    d="M64 32C28.7 32 0 60.7 0 96V416c0 35.3 28.7 64 64 64H384c35.3 0 64-28.7 64-64V96c0-35.3-28.7-64-64-64H64zm297.1 84L257.3 234.6 379.4 396H283.8L209 298.1 123.3 396H75.8l111-126.9L69.7 116h98l67.7 89.5L313.6 116h47.5zM323.3 367.6L153.4 142.9H125.1L296.9 367.6h26.3z" />
                </svg></a>
            </div>
          </section>
        </div>
        <div>
          <h3 class="font-bold">Autores</h3>
          <section class="bg-gray-100 shadow-slate-100 shadow-md border-gray-900 p-5 rounded-md flex flex-wrap gap-2">
            <article v-for="autor in getAutors" :key="autor"
              class="border border-black text-center capitalize px-3 py-1 rounded-sm">
              <p class="p-0 m-0 text-sm">{{ autor }}</p>
            </article>
          </section>
        </div>
        <div>
          <h3 class="font-bold">Estados</h3>
          <section
            class="bg-gray-100 shadow-slate-100 shadow-md border-gray-900 p-5 flex flex-wrap rounded-md gap-4 py-4 justify-center">

            <span :class="status" v-for="(status, key) in statusColor"
              class="px-3 py-1 rounded-md text-center  text-gray-600 ring-1 ring-inset ring-gray-500/10 italic text-sm font-bold capitalize h-fit w-10/12">
              {{ key }}
            </span>
          </section>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';
import Swal from 'sweetalert2';
import Comentarios from '../components/Comentarios.vue';
import Calificaciones from '../components/Calificaciones.vue';
const comentarioIsOpen = ref([]);
const calificacionIsOpen = ref([]);
const comments = ref([]);
const publications = ref([]);
const searchTerm = ref('');
const filterBy = ref('title');
const userRol = ref(null);  // Rol del usuario actual
const selectedPublicationId = ref(null);
const idUsuario = ref('holita');
const usuario = ref(''); // Asegúrate de asignar el ID del usuario
const comentario = ref('');  // Hacer el campo reactivo
const selectedCalification = ref(null);
const califications = ref([]);

const statusColor = {
  "aceptado": "bg-lime-300",
  "bloqueado": "bg-red-50",
  "revision": "bg-yellow-300"
}

// Obtener rol del usuario actual
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;  // Asignar rol del usuario
    idUsuario.value = user.id;
    usuario.value = user.userName;
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
    if (!isUserModerator) return true
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
      const response = await axios.put(`/api/Publications/UpdateStatus/${idpublicacion}`, {
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
const openComentarios = (id) => {
  const index = comentarioIsOpen.value.indexOf(id);
  index == -1 ? comentarioIsOpen.value.push(id) : comentarioIsOpen.value.splice(index, 1);
  console.log(comentarioIsOpen.value)
}
const openCalificaciones = (id) => {
  const index = calificacionIsOpen.value.indexOf(id);
  index == -1 ? calificacionIsOpen.value.push(id) : calificacionIsOpen.value.splice(index, 1);
  console.log(calificacionIsOpen.value)
}


// Seleccionar publicación y obtener los comentarios
const selectPublication = (id) => {
  selectedPublicationId.value = id;
  comments.value = []; // Limpiar comentarios
  califications.value = []; // Limpiar calificaciones
  //TODO quitar esto y pasarlo a componentes
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
  alert('Comentario guardado satisfactoriamente')
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
    console.log('Usuario actual:', idUsuario.value);

    // Verifica si el nombre de usuario actual está en la lista
    userHasLiked.value = usersWhoLiked.some(user => user.userName === usuario.value);
    console.log('Estado de Me gusta:', userHasLiked.value);
  } catch (error) {
    console.error('Error al obtener "Me gusta":', error.response ? error.response.data : error.message);
  }
};

const getAutors = computed(() => {
  const nombresVistos = new Set();
  filteredPublications.value.filter(publicaciones => {
    if (!nombresVistos.has(publicaciones.author)) {
      nombresVistos.add(publicaciones.author);
      return true; // Incluye el objeto
    }
    return false; // Excluye el objeto
  });
  return nombresVistos;
})

</script>


<style scoped>
ul {
  list-style-type: none;
  padding: 0;
}
</style>

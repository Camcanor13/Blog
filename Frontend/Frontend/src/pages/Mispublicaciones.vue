<template>
  <AgregarPublicacion/>
  <div>
    <div v-if="isEditing">
      <form @submit.prevent="saveChanges" class="fixed z-10 bg-white border border-black p-4">
        <label>Título:</label>
        <input v-model="editPublication.title" type="text" />
        
        <label>Cuerpo:</label>
        <textarea v-model="editPublication.body"></textarea>

        <!-- Mostrar selector de estado solo si la publicación está bloqueada -->
        <div v-if="editPublication.status === 'bloqueado'">
          <label>Estado:</label>
          <select v-model="editPublication.status">
            <option value="bloqueado">Bloqueado</option>
            <option value="revision">Revisión</option>
          </select>
        </div>

        <button type="submit">Guardar Cambios</button>
        <button @click="cancelEdit">Cancelar</button>
      </form>
    </div>

    <ul>
      <li v-for="publication in filteredPublications" :key="publication.id" class="border-y border-y-black p-5">
        <h2>Título: {{ publication.title }}</h2>
        <h3>Autor: {{ publication.author }}</h3>  
        <h3>Fecha de Publicación: {{ formatDate(publication.date) }}</h3>
        <h3>Estado: {{ publication.status }}</h3>
        <p>{{ publication.body }}</p>
        <h3>Comentarios: {{ publication.comments }}</h3>
        <h3>Calificación: {{ publication.qualification }}</h3>
        <button @click="editPublicationHandler(publication)" class="border border-black">Editar Publicación</button>
        <button @click="deletePublication(publication.id)" class="border border-black">Eliminar</button>
      </li>
      <p v-if="filteredPublications.length == 0">No hay publicaciones Disponibles</p>
    </ul>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import AgregarPublicacion from '../components/AgregarPublicacion.vue';

const publications = ref([]);
const userName = ref('');
const isEditing = ref(false);
const editPublication = ref({});

// Función para manejar la obtención de las publicaciones
const imprimir = async () => {
  try {
    const response = await axios.get('/api/Publications');
    publications.value = response.data;
  } catch (error) {
    console.error('Error al obtener las publicaciones:', error);
  }
};

// Función para eliminar una publicación
const deletePublication = async (idpublicacion) => {
  try {
    const response = await axios.delete(`/api/DeletePublication/${idpublicacion}`);
    alert(response.data);
    console.log(response.data); 
    imprimir();
  } catch (error) {
    alert(error.response.data);
    console.error(error.response.data);
  }
};

// Obtener las publicaciones y el nombre de usuario al montar el componente
onMounted(() => {
  imprimir();
  
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userName.value = user.userName;
  }
});

// Computed property para filtrar las publicaciones por autor
const filteredPublications = computed(() => {
  return publications.value.filter(publication => publication.author === userName.value);
});

// Función para formatear la fecha
const formatDate = (dateString) => {
  const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
  return new Date(dateString).toLocaleDateString(undefined, options);
};

// Manejar el clic en el botón de edición
const editPublicationHandler = (publication) => {
  isEditing.value = true;
  editPublication.value = { ...publication }; // Clonar el objeto para edición
};

// Guardar los cambios realizados
const saveChanges = async () => {
  try {
    const response = await axios.put(`/api/EditPublications/${editPublication.value.id}`, {
      title: editPublication.value.title,
      body: editPublication.value.body,
      date: editPublication.value.date,
      comments: editPublication.value.comments,
      qualification: editPublication.value.qualification,
      status: editPublication.value.status // Incluir el estado en la actualización
    });
    if (response.status === 200) {
      alert('Publicación actualizada exitosamente');
      isEditing.value = false;
      await imprimir(); // Volver a cargar las publicaciones actualizadas
    }
  } catch (error) {
    console.error('Error al actualizar la publicación:', error);
    alert('Error al actualizar la publicación');
  }
};

// Cancelar la edición
const cancelEdit = () => {
  isEditing.value = false;
};
</script>

<style>
/* Estilos opcionales */
</style>

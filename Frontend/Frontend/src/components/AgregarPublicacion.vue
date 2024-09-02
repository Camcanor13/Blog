<template>
  <form @submit.prevent="Agregar" class="border rounded-md p-4">
    <label>Título:</label>
    <input v-model="title" type="text" placeholder="Ingrese el título" class="block w-full border mb-2 p-2">

    <label>Cuerpo:</label>
    <textarea v-model="body" placeholder="Información de la publicación" class="block w-full border mb-2 p-2"></textarea>

    <button type="submit" class="bg-blue-500 text-white p-2 rounded">Agregar</button>
  </form>
</template>


<script setup>
import { onMounted, ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const title = ref('');
const body = ref('');
const date = ref(new Date().toISOString()); 
const comments = ref(0); 
const qualification = ref(0); 
const status = ref('revision');
const autor = ref('');

// Obtener el autor desde el localStorage
const userData = localStorage.getItem('userData');
if (userData) {
  const user = JSON.parse(userData);
  autor.value = user.id; 
}

const router = useRouter();

const Agregar = async () => {
    if (!title.value.trim() || !body.value.trim()) {
    alert('El título y el cuerpo no pueden estar vacíos.');
    return; // Detener el envío si los campos están vacíos
  }
  try {
    // Enviar una solicitud POST al backend con los datos del formulario
    const response = await axios.post('/api/AddPublications', {
      title: title.value,
      body: body.value,
      date: date.value,
      comments: comments.value,
      qualification: qualification.value,
      status: status.value,
      author: autor.value,
    });

    if (response.status === 200) {
      alert('Publicación agregada exitosamente.');
      router.push('/dashboard');
    } else {
      alert('Hubo un problema al agregar la publicación.');
    }
  } catch (error) {
    console.error('Error al agregar la publicación:', error);
    alert(error.response.data);
  }
};

</script>



<style scoped>

</style>

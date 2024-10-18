<template>
    <section class="flex flex-wrap py-2">
        <article class="p-2 sm:w-1/4 border-r border-gray rounded-md" v-if="props.userType" >
            <StarCalifications @updateRating="obtenerCalificacion" :rating="miCalificacion" />
            <h3 class="text-sm font-medium text-gray-700 sm:mt-1 text-right">&mdash;yo</h3>
        </article>
        <article class="p-2 sm:w-1/4 border-r border-gray rounded-md" v-for="calificacion in calificaciones">
            <StarCalifications :canModify="false" :rating="calificacion.calification"/>
            <h3 class="text-sm font-medium text-gray-700 sm:mt-1 text-right">&mdash;{{ calificacion.userName }}</h3>
        </article>
    </section>
    
</template>

<script setup>
import { ref, computed,onMounted } from 'vue';
import axios from 'axios';
import StarCalifications from './StarCalifications.vue';
import Swal from 'sweetalert2';

const props = defineProps({
    idpublicacion: {
        type: Number,
        required: true,
        default: -1
    },idUsuario:{
        type: Number,
        required: true,
    },userName:{
        type: String,
        required: true,
    },userType:{
        type:Boolean,
        required:true
    }
});
onMounted(() => {
 imprimirCalifications();
});

const calificaciones=ref([]);
const miCalificacion=ref(0);

const obtenerCalificacion= async (value) => {
    miCalificacion.value = value;
    try {
        console.log(value,"calificacion")
    const response = await axios.post(`/api/AddCalification`, {
      id_publicacion: props.idpublicacion,
      id_usuario: props.idUsuario,
      calificacion: value
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

const imprimirCalifications = async () => {
    try {
        const response = await axios.get(`/api/ListCalification/${props.idpublicacion}`);
        console.log('Calificaciones:', response.data);
        calificaciones.value=response.data.filter((calificacion,idx) =>{
            if(calificacion.userName==props.userName){
                miCalificacion.value=calificacion.calificacion
                return null
            }
            return calificacion
        })
        // = response.data;
        // let idxMC=-1;
        // calificaciones.value.((calificacion,idx) => {
        //     if(calificacion.userName==props.userName){
        //         miCalificacion.value=calificacion.calificacion
        //         idxMC=idx
        //     }
        // });
        // calificacion.
    } catch (error) {
        console.error('Error al obtener las calificaciones:', error);
    }
};
</script>
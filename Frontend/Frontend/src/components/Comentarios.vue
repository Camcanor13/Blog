<template>
    <section class="w-full grid gap-4 justify-items-center py-3 max-h-[40vh] overflow-auto">
        <h2 class="text-center text-xl" v-if="comentarios.length == 0">sé el primero en dar un comentario</h2>
        <blockquote
            class="flex h-full w-full flex-col justify-between rounded-lg bg-white p-6 sm:py-2 py-1 shadow-sm sm:p-4">
            <div>
                <div class="my-1 flex flex-wrap gap-2">
                    <input type="text" name="" ref="comentariousuario"
                        class="mt-1 leading-relaxed text-gray-700 border border-gray-300 rounded-md w-full px-4">
                </div>
            </div>

            <footer class="mt-1 text-sm font-medium text-gray-700 sm:mt-1 text-right">
                <button class="border border-gray-300 rounded-md px-4" @click="submitComment()">agregar</button>
            </footer>
        </blockquote>
        <blockquote v-for="comentario in comentarios"
            class="flex h-full w-full flex-col justify-between rounded-lg bg-white p-6 sm:py-2 py-1 shadow-sm sm:p-4">
            <div>
                <div class="my-1">
                    <p class="mt-1 leading-relaxed text-gray-700">{{ comentario.comment }}</p>
                </div>
            </div>

            <footer class="mt-1 text-sm font-medium text-gray-700 sm:mt-1 text-right">
                &mdash; {{ comentario.userName }}
            </footer>
        </blockquote>
        
    </section>
</template>

<script>

import axios from 'axios';
export default {
    props: {
        idpublicacion: {
            type: Number,
            required: true,
            default: -1
        }, idUsuario: {
            type: Number,
            required: false
        }
    },
    watch: {
        idpublicacion(newv, oldv) {
            imprimirComentarios()
        }
    }, data() {
        return { comentarios: [] }
    },
    mounted() {
        this.imprimirComentarios();
    },
    methods: {
        async imprimirComentarios() {
            try {
                const response = await axios.get(`/api/ListComment?idpublicacion=${this.idpublicacion}`);
                console.log(response.data)
                this.comentarios = response.data;
            } catch (error) {
                console.error('Error al obtener los comentarios:', error);
            }
        },
        async submitComment () {


            if (this.$refs.comentariousuario.value.trim() === '') {
                alert('El campo de comentario está vacío.');
                return; // No continuar si el comentario está vacío
            }

            try {
                console.log('Enviando comentario:', {
                    id_publicacion: this.idpublicacion,
                    id_usuario: this.idUsuario,
                    comentario: this.$refs.comentariousuario.value
                });

                const response = await axios.post(`/api/AddComment`, {
                    id_publicacion: this.idpublicacion,
                    id_usuario: this.idUsuario,
                    comentario: this.$refs.comentariousuario.value
                });

                alert('Comentario guardado satisfactoriamente')
                this.$refs.comentariousuario.value="";
                console.log('Comentario enviado:', response.data);
                comentario.value = '';  // Limpiar el campo después de enviar
                await this.imprimirComentarios();  // Actualizar comentarios
            } catch (error) {
                console.error('Error al enviar el comentario:', error.response ? error.response.data : error.message);
            }
        }
    },
}


</script>
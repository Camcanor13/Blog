<template>
  <div class="flex items-center justify-between border border-gray-300 px-5 m-0  shadow-md bg-sapphire-blue">
    <!-- Sección de permisos -->
    <div class="flex items-center space-x-4">
      <p class="text-sm text-faint-red font-medium">
        Tu cuenta tiene permisos de: <span class="font-semibold">{{ tipoRol }}</span>
      </p>
      <p class="text-sm text-faint-red font-medium">
        Usuario: <span class="font-semibold">{{ nombreUsuario }}</span>
      </p>
    </div>

    <!-- Icono de usuario y menú desplegable
    <div class="relative">
      <button @click="toggleDropdown" class="flex items-center p-2 rounded-full hover:bg-gray-700">
        <svg class="h-6 w-6 text-gray-300" fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path
            d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z" />
        </svg>
      </button>
      <div v-if="showDropdown" class="absolute right-0 mt-2 py-2 w-48 bg-white rounded-md shadow-xl z-20">
        <a href="/dashboard/perfil" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Perfil</a>
        <a @click="logout" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 cursor-pointer">Cerrar
          sesión</a>
      </div>
    </div>
     -->
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const nombreUsuario = ref('');
const tipoRol = ref('');
const router = useRouter();
const showDropdown = ref(false);

onMounted(() => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    nombreUsuario.value = user.userName;
    tipoRol.value = user.rol === "1" ? "Autor" : user.rol === "2" ? "Moderador" : "Lector";
  }
});

const logout = () => {
  localStorage.removeItem('userData');
  alert("Se cerró sesión exitosamente :)");
  router.push('/');
};

const toggleDropdown = () => {
  showDropdown.value = !showDropdown.value;
};
</script>

<style scoped>
.bg-sapphire-blue {
  background-color: #0f4c81;
  /* Azul Zafiro */
}

.text-faint-red {
  color: #0f0e0e;
  /* Rojo Tenue */
}
</style>

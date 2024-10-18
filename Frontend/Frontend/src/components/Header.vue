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

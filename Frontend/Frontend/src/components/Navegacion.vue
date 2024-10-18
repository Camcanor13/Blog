<template>
  <Disclosure as="nav" class="bg-gray-800" v-slot="{ open }">
    <div class="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
      <div class="relative flex h-16 items-center justify-between">
        <div class="absolute inset-y-0 left-0 flex items-center sm:hidden">
          <!-- Mobile menu button
          <DisclosureButton class="relative inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white">
            <span class="absolute -inset-0.5" />
            <span class="sr-only">Open main menu</span>
            <Bars3Icon v-if="!open" class="block h-6 w-6" aria-hidden="true" />
            <XMarkIcon v-else class="block h-6 w-6" aria-hidden="true" />
          </DisclosureButton> 
          -->
        </div>
        <div class="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
          <div class="flex flex-shrink-0 items-center">
            <img class="h-8 w-auto" src="../../public/logo.png" alt="" />
            <router-link to="/dashboard/inicio">
              <h1 class="text-xl text-white"><span class="font-bold">La pluma </span>Digital</h1>
            </router-link>
          </div>
          <div class="hidden sm:ml-6 sm:block">
            <div class="flex space-x-4">
              <a v-for="item in navigation" :key="item.name" :href="item.href"
                :class="[item.current ? 'bg-gray-900 text-white' : 'text-gray-300 hover:bg-gray-700 hover:text-white', 'rounded-md px-3 py-2 text-sm font-medium']"
                :aria-current="item.current ? 'page' : undefined">{{ item.name }}</a>
            </div>
          </div>
        </div>
        <div class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
          <button type="button"
            class="relative rounded-full bg-gray-800 p-1 text-gray-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800">
            <span class="absolute -inset-1.5" />
            <span class="sr-only">View notifications</span>
            <BellIcon class="h-6 w-6" aria-hidden="true" />
          </button>

          <!-- Profile dropdown -->
          <Menu as="div" class="relative ml-3">
            <div>
              <MenuButton
                class="relative flex rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-gray-800 hover:bg-white">
                <span class="absolute -inset-1.5"></span>
                <span class="sr-only">Open user menu</span>
                <img class="h-8 w-8 rounded-full"
                  src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                  alt="" />
              </MenuButton>
            </div>
            <transition enter-active-class="transition ease-out duration-100"
              enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100"
              leave-active-class="transition ease-in duration-75" leave-from-class="transform opacity-100 scale-100"
              leave-to-class="transform opacity-0 scale-95">
              <MenuItems
                class="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                <MenuItem>
                <a @click="logout" class="bg-gray-100 block px-4 py-2 text-sm text-gray-700 cursor-pointer">Cerrar
                  Sesion</a>
                </MenuItem>
              </MenuItems>
            </transition>
          </Menu>
        </div>
      </div>
    </div>

    <DisclosurePanel class="sm:hidden">
      <div class="space-y-1 px-2 pb-3 pt-2">
        <DisclosureButton v-for="item in navigation" :key="item.name" as="a" :href="item.href"
          :class="[item.current ? 'bg-gray-900 text-white' : 'text-gray-300 hover:bg-gray-700 hover:text-white', 'block rounded-md px-3 py-2 text-base font-medium']"
          :aria-current="item.current ? 'page' : undefined">{{ item.name }}</DisclosureButton>
      </div>
    </DisclosurePanel>
  </Disclosure>
</template>

<script setup>
import { Disclosure, DisclosureButton, DisclosurePanel, Menu, MenuButton, MenuItem, MenuItems } from '@headlessui/vue'
import { Bars3Icon, BellIcon, XMarkIcon } from '@heroicons/vue/24/outline';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const userRol = ref('');
const nombreUsuario = ref('');
const tipoRol = ref('');
const router = useRouter();
const showDropdown = ref(false);

// Función para obtener el rol del usuario desde el localStorage
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
}
onMounted(() => {
  getUserRole();
});

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

const navigation = [
  { name: 'Inicio', href: '/dashboard/inicio', current: true },
  { name: 'Publicaciones', href: '/dashboard/publicaciones', current: false },
]
</script>

<style>
/* Agrega tus estilos personalizados aquí */
</style>

<!-- <template>
  <nav class="bg-white shadow-md">
    <div class="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
      <div class="flex">
        <img src="../../public/logo.png" class="w-8 h-8 p-1">
        <router-link to="/dashboard/inicio">
          <h1 class="text-xl "><span class="font-bold">La pluma </span>Digital</h1>
        </router-link>

      </div>
      <div>
        <ul class="list-none p-0 md:flex md:space-x-4">
          <li>
            <router-link to="/dashboard/inicio"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Inicio
            </router-link>
          </li>
          <li>
            <router-link to="/dashboard/about"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              About Us
            </router-link>
          </li>
          <li v-if="userRol === '1'">
            <router-link to="/dashboard/publicaciones"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Mis publicaciones
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const userRol = ref('');

// Función para obtener el rol del usuario desde el localStorage
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
};

onMounted(() => {
  getUserRole();
});
</script>
-->

<style>
/* Agrega tus estilos personalizados aquí */
</style>


<!--<template>
  <nav class="bg-white shadow-md">
    <div class="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
      <div class="flex">
        <img src="../../public/logo.png" class="w-8 h-8 p-1">
        <router-link to="/dashboard/inicio">
          <h1 class="text-xl "><span class="font-bold">La pluma </span>Digital</h1>
        </router-link>

      </div>
      <div>
        <ul class="list-none p-0 md:flex md:space-x-4">
          <li>
            <router-link to="/dashboard/inicio"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Inicio
            </router-link>
          </li>
          <li>
            <router-link to="/dashboard/about"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              About Us
            </router-link>
          </li>
          <li v-if="userRol === '1'">
            <router-link to="/dashboard/publicaciones"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Mis publicaciones
            </router-link>
          </li>
          <li v-if="userRol === '3'">
            <router-link to="/dashboard/lista"
              class="text-gray-800 hover:text-blue-500 px-3 py-2 rounded-md text-sm font-medium">
              Listado Me Gusta
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const userRol = ref('');

// Función para obtener el rol del usuario desde el localStorage
const getUserRole = () => {
  const userData = localStorage.getItem('userData');
  if (userData) {
    const user = JSON.parse(userData);
    userRol.value = user.rol;
  }
};

onMounted(() => {
  getUserRole();
});
</script>

<style>
/* Agrega tus estilos personalizados aquí */
</style>
-->
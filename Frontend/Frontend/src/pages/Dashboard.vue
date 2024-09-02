<template>
  <Header/>
  <Navegacion/>
   <div >
   <router-view></router-view>
 </div>
</template>

<script setup>

import Header from '../components/Header.vue';
import Navegacion from '../components/Navegacion.vue'


</script>

<style>

</style>

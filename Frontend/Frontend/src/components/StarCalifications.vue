<template>
    <div class="flex items-center">
      <svg
        v-for="star in maxStars"
        v-if="props.canModify"
        :key="star"
        @click="setRating(star)"
        @mouseenter="hoverRating = star"
        @mouseleave="hoverRating = 0"
        :class="starClass(star)"
        xmlns="http://www.w3.org/2000/svg"
        fill="currentColor"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        class="w-8 h-8 cursor-pointer"
      >
        <path
          d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"
        />
      </svg>
      <svg
        v-for="star in maxStars"
        v-if="!props.canModify"
        :key="star"
        :class="starClass(star)"
        xmlns="http://www.w3.org/2000/svg"
        fill="currentColor"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        class="w-8 h-8 cursor-pointer"
      >
        <path
          d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"
        />
      </svg>
    </div>
  </template>
  
  <script setup>
  import { ref, computed,defineEmits } from 'vue';
  
  const props = defineProps({
    maxStars: {
      type: Number,
      default: 5,
    },rating:{
        type:Number,
        default: 0,
    },canModify:{
        type:Boolean,
        default:true
    }
  });
  const hoverRating = ref(0);
  const emitRating = defineEmits(['updateRating']);

  const setRating = (star) => {
    props.rating = star;
    emitRating('updateRating', star);
  };
  
  const starClass = (star) => {
    if (hoverRating.value >= star || props.rating >= star) {
      return 'text-yellow-400';
    }
    return 'text-gray-300';
  };
  </script>
  
  <style scoped>
  svg:hover {
    transition: all 0.2s ease;
  }
  </style>
  
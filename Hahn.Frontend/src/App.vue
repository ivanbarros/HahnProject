<!-- src/App.vue -->

<template>
  <div id="app">
    <NavigationBar />
    <router-view />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import NavigationBar from './components/NavigationBar.vue';

  export default defineComponent({
    name: 'App',
    components: {
      NavigationBar,
    },
  });
</script>

<style>
  /* Global styles can be added here or in global.css */
</style>

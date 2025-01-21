<!-- src/components/NavigationBar.vue -->

<template>
  <nav class="navbar">
    <router-link to="/recipes" class="nav-link">Home</router-link>
    <router-link to="/upsert" class="nav-link">Create Recipe</router-link>
  </nav>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'NavigationBar',
  });
</script>

<style scoped>
  .navbar {
    background-color: #333;
    padding: 10px 20px;
    display: flex;
    gap: 20px;
  }

  .nav-link {
    color: white;
    text-decoration: none;
    font-size: 18px;
  }

    .nav-link:hover {
      text-decoration: underline;
    }
</style>

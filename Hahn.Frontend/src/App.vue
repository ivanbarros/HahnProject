<template>
  <div id="app" class="app-container">
   
    <header class="app-header">
      <h1 class="app-title">My Great Recipies</h1>
      <p class="app-subtitle">Discover, filter, and enjoy your perfect meal!</p>
    </header>

   
    <main class="app-main">
      <section class="grid-section">
        <RecipeGrid />
      </section>
    </main>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import RecipeGrid from './components/RecipeGrid.vue'

  export default defineComponent({
    name: 'App',
    components: {
      RecipeGrid
    }
  })
</script>

<style scoped>
 
  .app-container {
    min-height: 100vh;
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
   
    background: linear-gradient(to bottom right, #f9f7f7 0%, #e5e5e5 100%);
   
    color: #333;
  }

  
  .app-header {
    text-align: center;
    padding: 2rem 1rem 1.5rem;
  }

  .app-title {
    font-size: 2rem;
    margin: 0.5rem 0;
    font-weight: bold;
  }

  .app-subtitle {
    font-size: 1rem;
    color: #555;
    margin: 0;
  }

 
  .app-main {
    display: flex;
    justify-content: center;
    align-items: flex-start;
    padding: 1rem;
  }

  
  .grid-section {
    width: 100%;
    max-width: 1200px;
    margin: 0 auto;
  }
</style>

<template>
  <div id="app">
    <h1>Hahn Project - Food Recipes</h1>
    <RecipeGrid />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue' 
  import RecipeGrid from './components/RecipeGrid.vue'

  export default defineComponent({
    name: 'App',
    components: { RecipeGrid }
  })
</script>

<style scoped>
  h1 {
    margin-bottom: 1rem;
  }
</style>

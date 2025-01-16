<template>
  <div>
    <div>
      <input v-model="filterTerm" placeholder="Filter by title..." />
      <button @click="fetchRecipes">Refresh</button>
    </div>

    <table>
      <thead>
        <tr>
          <th>Title</th>
          <th>Ingredients</th>
          <th>Instructions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="recipe in filteredRecipes" :key="recipe.id">
          <td>{{ recipe.title }}</td>
          <td>{{ recipe.ingredients }}</td>
          <td>{{ recipe.instructions }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted, computed } from 'vue'
  import axios from 'axios'

  interface RecipeDto {
    id: number
    title: string
    ingredients: string
    instructions: string
  }

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      const recipes = ref<RecipeDto[]>([])
      const filterTerm = ref('')

      const fetchRecipes = async () => {
        const response = await axios.get<RecipeDto[]>('https://localhost:7105/api/recipes')
        recipes.value = response.data
      }

      const filteredRecipes = computed(() => {
        if (!filterTerm.value.trim()) return recipes.value
        return recipes.value.filter(r =>
          r.title.toLowerCase().includes(filterTerm.value.toLowerCase())
        )
      })

      onMounted(fetchRecipes)

      return {
        recipes,
        filterTerm,
        filteredRecipes,
        fetchRecipes
      }
    }
  })
</script>

<style scoped>
  table {
    border-collapse: collapse;
    width: 100%;
  }

  th, td {
    border: 1px solid #ccc;
    padding: 0.5rem;
  }
</style>

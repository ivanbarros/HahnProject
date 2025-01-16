<template>
  <div class="page-container">
   
    <header class="page-header">
      <h1 class="page-title">Delicious Recipes</h1>
      <p class="page-subtitle">Find and filter recipes for your next meal!</p>
    </header>

   
    <main>
      <section class="filter-section">
        <label for="recipeFilter" class="filter-label">Search by Title</label>
        <input id="recipeFilter"
               class="filter-input"
               v-model="filterTerm"
               placeholder="Filter by title..." />
        <button class="filter-button" @click="fetchRecipes">
          Refresh
        </button>
      </section>

     
      <section class="table-section">
        <table class="recipe-table">
          <thead>
            <tr>
              <th scope="col">Title</th>
              <th scope="col">Ingredients</th>
              <th scope="col">Instructions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="recipe in filteredRecipes"
                :key="recipe.id">
              <td>{{ recipe.title }}</td>
              <td>{{ recipe.ingredients }}</td>
              <td>{{ recipe.instructions }}</td>
            </tr>
          </tbody>
        </table>
      </section>
    </main>
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
        if (!filterTerm.value.trim()) {
          return recipes.value
        }
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

  .page-container {
    font-family: Arial, sans-serif;
    background:
   
    linear-gradient(to bottom, #fdfcfb 0%, #e2d1c3 100%);
   
  background-size: cover;
  
    min-height: 100vh;
    padding: 2rem;
    color: #333;
  }

 
  .page-header {
    text-align: center;
    margin-bottom: 2rem;
  }

  .page-title {
    font-size: 2rem;
    margin: 0.5rem 0;
    font-weight: bold;
  }

  .page-subtitle {
    font-size: 1rem;
    color: #555;
  }
 
  .filter-section {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 0.5rem;
    margin-bottom: 1rem;
    justify-content: center;
  }

  .filter-label {
    font-weight: 600;
  }

  .filter-input {
    flex: 1 1 250px;
    padding: 0.5rem;
    border: 1px solid #aaa;
    border-radius: 4px;
  }

  .filter-button {
    padding: 0.5rem 1rem;
    background-color: #ff6700;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 600;
  }

    .filter-button:hover {
      background-color: #e65c00;
    }

 
  .table-section {
    margin-top: 1.5rem;
    overflow-x: auto;
  }

  
  .recipe-table {
    width: 100%;
    border-collapse: collapse;
    text-align: left;
    background-color: #fff;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
  }

    .recipe-table thead {
      background-color: #f2f2f2;
    }

    .recipe-table th,
    .recipe-table td {
      padding: 0.75rem 1rem;
      border: 1px solid #ccc;
    }

    .recipe-table tr:hover {
      background-color: #fafafa;
    }
</style>

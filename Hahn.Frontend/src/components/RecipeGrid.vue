<template>
  <div class="page-container">
    <!-- Page Header -->
    <header class="page-header">
      <h1 class="page-title">Delicious Recipes</h1>
      <p class="page-subtitle">Find and filter recipes for your next meal!</p>
    </header>

    <!-- Main Content -->
    <main>
      <!-- Filter & Refresh Section (GET all recipes) -->
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

      <!-- Recipes Table -->
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

      <!-- Search By ID (GET /api/recipes/{id}) -->
      <section class="search-id-section">
        <h2>Find Recipe by ID</h2>
        <input class="id-input"
               v-model.number="singleId"
               type="number"
               placeholder="Enter recipe ID..." />
        <button class="action-button" @click="fetchRecipeById">
          Search
        </button>
        <div v-if="singleRecipe" class="single-recipe-result">
          <h3>Recipe Found:</h3>
          <p><strong>Title:</strong> {{ singleRecipe.title }}</p>
          <p><strong>Ingredients:</strong> {{ singleRecipe.ingredients }}</p>
          <p><strong>Instructions:</strong> {{ singleRecipe.instructions }}</p>
        </div>
        <div v-if="singleRecipeError" class="error-message">
          {{ singleRecipeError }}
        </div>
      </section>

      <!-- Upsert Recipe (POST /api/recipes/upsert) -->
      <section class="upsert-section">
        <h2>Create/Update Recipe</h2>
        <input class="upsert-input"
               v-model="upsertTitle"
               placeholder="Recipe Title" />
        <input class="upsert-input"
               v-model="upsertIngredients"
               placeholder="Ingredients" />
        <input class="upsert-input"
               v-model="upsertInstructions"
               placeholder="Instructions" />
        <button class="action-button" @click="upsertRecipe">
          Upsert
        </button>
        <p class="hint">
          If ID is required for update, add it in the code <br />
          or modify your endpoint to handle an ID field in the request body.
        </p>
        <div v-if="upsertError" class="error-message">
          {{ upsertError }}
        </div>
        <div v-if="upsertSuccess" class="success-message">
          {{ upsertSuccess }}
        </div>
      </section>

      <!-- Delete Recipe (DELETE /api/recipes/delete/{id}) -->
      <section class="delete-section">
        <h2>Delete Recipe by ID</h2>
        <input class="id-input"
               v-model.number="deleteId"
               type="number"
               placeholder="Enter recipe ID..." />
        <button class="action-button delete-button" @click="deleteRecipe">
          Delete
        </button>
        <div v-if="deleteError" class="error-message">
          {{ deleteError }}
        </div>
        <div v-if="deleteSuccess" class="success-message">
          {{ deleteSuccess }}
        </div>
      </section>
    </main>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted, computed } from 'vue'
  import axios from 'axios'

  interface RecipeDto {
    id?: number
    title: string
    ingredients: string
    instructions: string
  }

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      // All recipes
      const recipes = ref<RecipeDto[]>([])
      // Filter text
      const filterTerm = ref('')

      // Single recipe search
      const singleId = ref<number | null>(null)
      const singleRecipe = ref<RecipeDto | null>(null)
      const singleRecipeError = ref<string | null>(null)

      // Upsert (create/update) recipe fields
      const upsertTitle = ref('')
      const upsertIngredients = ref('')
      const upsertInstructions = ref('')
      const upsertError = ref<string | null>(null)
      const upsertSuccess = ref<string | null>(null)

      // Delete recipe
      const deleteId = ref<number | null>(null)
      const deleteError = ref<string | null>(null)
      const deleteSuccess = ref<string | null>(null)

      // 1. GET: Fetch all recipes
      const fetchRecipes = async () => {
        try {
          const response = await axios.get<RecipeDto[]>('https://localhost:7105/api/recipes')
          recipes.value = response.data
        } catch (error) {
          console.error('Error fetching recipes:', error)
        }
      }

      // 2. GET: Fetch recipe by ID
      const fetchRecipeById = async () => {
        singleRecipeError.value = null
        singleRecipe.value = null

        if (singleId.value === null) {
          singleRecipeError.value = 'Please enter a valid recipe ID.'
          return
        }

        try {
          const response = await axios.get<RecipeDto>(`https://localhost:7105/api/recipes/${singleId.value}`)
          singleRecipe.value = response.data
        } catch (error: any) {
          console.error('Error fetching recipe by ID:', error)
          if (error.response && error.response.status === 404) {
            singleRecipeError.value = `Recipe with ID ${singleId.value} not found.`
          } else {
            singleRecipeError.value = 'An error occurred while fetching the recipe.'
          }
        }
      }

      // 3. POST: Upsert recipe
      //    Depending on your API, the same endpoint might handle both create & update.
      const upsertRecipe = async () => {
        upsertError.value = null
        upsertSuccess.value = null

        try {
          const dataToSend = {
            title: upsertTitle.value,
            ingredients: upsertIngredients.value,
            instructions: upsertInstructions.value
            // If your API expects an ID for update, you can add an 'id' field here
            // id: ...
          }
          const response = await axios.post<RecipeDto>('https://localhost:7105/api/recipes/upsert', dataToSend)
          // Refresh the main list after upsert
          await fetchRecipes()
          // Clear form
          upsertTitle.value = ''
          upsertIngredients.value = ''
          upsertInstructions.value = ''
          upsertSuccess.value = 'Recipe upserted successfully!'
        } catch (error: any) {
          console.error('Error upserting recipe:', error)
          upsertError.value = 'An error occurred while upserting the recipe.'
        }
      }

      // 4. DELETE: Delete recipe by ID
      const deleteRecipe = async () => {
        deleteError.value = null
        deleteSuccess.value = null

        if (deleteId.value === null) {
          deleteError.value = 'Please enter a valid recipe ID.'
          return
        }

        try {
          await axios.delete(`https://localhost:7105/api/recipes/delete/${deleteId.value}`)
          // Refresh after deletion
          await fetchRecipes()
          deleteId.value = null
          deleteSuccess.value = 'Recipe deleted successfully!'
        } catch (error: any) {
          console.error('Error deleting recipe:', error)
          if (error.response && error.response.status === 404) {
            deleteError.value = `Recipe with ID ${deleteId.value} not found.`
          } else {
            deleteError.value = 'An error occurred while deleting the recipe.'
          }
        }
      }

      // Filter logic for table
      const filteredRecipes = computed(() => {
        if (!filterTerm.value.trim()) {
          return recipes.value
        }
        return recipes.value.filter(r =>
          r.title.toLowerCase().includes(filterTerm.value.toLowerCase())
        )
      })

      // Fetch initial list on mount
      onMounted(fetchRecipes)

      return {
        recipes,
        filterTerm,
        filteredRecipes,
        fetchRecipes,
        singleId,
        singleRecipe,
        singleRecipeError,
        fetchRecipeById,
        upsertTitle,
        upsertIngredients,
        upsertInstructions,
        upsertRecipe,
        upsertError,
        upsertSuccess,
        deleteId,
        deleteRecipe,
        deleteError,
        deleteSuccess
      }
    }
  })
</script>

<style scoped>
  .page-container {
    font-family: Arial, sans-serif;
    background: linear-gradient(to bottom, #fdfcfb 0%, #e2d1c3 100%);
    min-height: 100vh;
    padding: 2rem;
    color: #333;
  }

  /* Header */
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

  /* Filter Section */
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

  /* Table Section */
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

  /* Search By ID, Upsert, Delete Sections */
  .search-id-section,
  .upsert-section,
  .delete-section {
    margin: 2rem 0;
  }

  .id-input,
  .upsert-input {
    width: 250px;
    padding: 0.5rem;
    margin-right: 0.5rem;
    border: 1px solid #aaa;
    border-radius: 4px;
  }

  .action-button {
    padding: 0.5rem 1rem;
    background-color: #007bff;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 600;
  }

    .action-button:hover {
      background-color: #0069d9;
    }

  .delete-button {
    background-color: #ff0000;
  }

    .delete-button:hover {
      background-color: #cc0000;
    }

  .single-recipe-result,
  .hint,
  .error-message,
  .success-message {
    margin-top: 1rem;
  }

  .error-message {
    color: red;
  }

  .success-message {
    color: green;
  }
</style>

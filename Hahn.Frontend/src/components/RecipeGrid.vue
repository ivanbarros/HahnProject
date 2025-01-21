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
            <tr v-for="recipe in filteredRecipes" :key="recipe.id">
              <td>{{ recipe.title }}</td>
              <td>{{ recipe.ingredients }}</td>
              <td>{{ recipe.instructions }}</td>
            </tr>
          </tbody>
        </table>
      </section>

      <!-- Search By Recipe Selection (GET /api/recipes/{id}) -->
      <section class="search-id-section">
        <h2>Find Recipe by Selection</h2>
        <select class="recipe-select" v-model="selectedRecipeId">
          <option disabled value="">-- Select a Recipe --</option>
          <option v-for="recipe in recipes"
                  :key="recipe.id"
                  :value="recipe.id">
            {{ recipe.title }}
          </option>
        </select>
        <button class="action-button"
                @click="fetchRecipeById"
                :disabled="!selectedRecipeId || isLoading">
          {{ isLoading ? 'Loading...' : 'Search' }}
        </button>

        <div v-if="singleRecipe" class="single-recipe-result">
          <h3>Recipe Found:</h3>
          <p><strong>Title:</strong> {{ singleRecipe.title }}</p>
          <p><strong>Ingredients:</strong> {{ singleRecipe.ingredients }}</p>
          <p><strong>Instructions:</strong> {{ singleRecipe.instructions }}</p>
          <button class="edit-button" @click="loadRecipeForUpdate">
            Edit Recipe
          </button>
        </div>
        <div v-if="singleRecipeError" class="error-message">
          {{ singleRecipeError }}
        </div>
      </section>

      <!-- Search for a Recipe Not in the Grid -->
      <section class="search-not-in-grid-section">
        <h2>Search for a Recipe</h2>
        <input class="search-input"
               v-model="searchTerm"
               type="text"
               placeholder="Enter recipe title..." />
        <button class="action-button"
                @click="searchRecipe"
                :disabled="!searchTerm.trim() || isSearching">
          {{ isSearching ? 'Searching...' : 'Search' }}
        </button>

        <div v-if="searchedRecipe" class="searched-recipe-result">
          <h3>Search Result:</h3>
          <p><strong>Title:</strong> {{ searchedRecipe.title }}</p>
          <p><strong>Ingredients:</strong> {{ searchedRecipe.ingredients }}</p>
          <p><strong>Instructions:</strong> {{ searchedRecipe.instructions }}</p>
        </div>
        <div v-if="searchError" class="error-message">
          {{ searchError }}
        </div>
      </section>

      <!-- Upsert Recipe (POST /api/recipes/upsert) -->
      <section class="upsert-section">
        <h2>Create/Update Recipe</h2>
        <!-- Hidden field for Upsert ID -->
        <input class="upsert-input"
               v-model="upsertId"
               type="hidden" />

        <input class="upsert-input"
               v-model="upsertTitle"
               type="text"
               placeholder="Recipe Title" />
        <input class="upsert-input"
               v-model="upsertIngredients"
               type="text"
               placeholder="Ingredients" />
        <input class="upsert-input"
               v-model="upsertInstructions"
               type="text"
               placeholder="Instructions" />
        <button class="action-button"
                @click="upsertRecipe"
                :disabled="!upsertTitle || !upsertIngredients || !upsertInstructions || isUpserting">
          {{ isUpserting ? 'Upserting...' : 'Upsert' }}
        </button>

        <div v-if="upsertError" class="error-message">
          {{ upsertError }}
        </div>
        <div v-if="upsertSuccess" class="success-message">
          {{ upsertSuccess }}
        </div>
      </section>

      <!-- Delete Recipe (DELETE /api/recipes/delete/{id}) -->
      <section class="delete-section">
        <h2>Delete Recipe by Selection</h2>
        <select class="recipe-select" v-model="selectedDeleteRecipeId">
          <option disabled value="">-- Select a Recipe to Delete --</option>
          <option v-for="recipe in recipes"
                  :key="recipe.id"
                  :value="recipe.id">
            {{ recipe.title }}
          </option>
        </select>
        <button class="action-button delete-button"
                @click="deleteRecipe"
                :disabled="!selectedDeleteRecipeId || isDeleting">
          {{ isDeleting ? 'Deleting...' : 'Delete' }}
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
  import api from '@/api' // Ensure this path is correct based on alias configuration

  interface RecipeDto {
    id: string; // Ensure this matches the backend's casing
    title: string;
    ingredients: string;
    instructions: string;
    description?: string;
    cuisine?: string;
  }

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      // All recipes
      const recipes = ref<RecipeDto[]>([])
      // Filter text
      const filterTerm = ref('')

      // Selected recipe ID from dropdown for fetching
      const selectedRecipeId = ref<string>('')
      const singleRecipe = ref<RecipeDto | null>(null)
      const singleRecipeError = ref<string | null>(null)
      const isLoading = ref(false)

      // Selected recipe ID from dropdown for deletion
      const selectedDeleteRecipeId = ref<string>('')
      const deleteError = ref<string | null>(null)
      const deleteSuccess = ref<string | null>(null)
      const isDeleting = ref(false)

      // Upsert (create/update) recipe fields
      const upsertId = ref<string>('') // Hidden field for ID
      const upsertTitle = ref('')
      const upsertIngredients = ref('')
      const upsertInstructions = ref('')
      const upsertError = ref<string | null>(null)
      const upsertSuccess = ref<string | null>(null)
      const isUpserting = ref(false)

      // Search for a recipe not in the grid
      const searchTerm = ref('')
      const searchedRecipe = ref<RecipeDto | null>(null)
      const searchError = ref<string | null>(null)
      const isSearching = ref(false)

      // Utility function to validate GUIDs
      const isValidGUID = (guid: string): boolean => {
        const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i
        return guidRegex.test(guid)
      }

      // 1. GET: Fetch all recipes
      const fetchRecipes = async () => {
        try {
          console.log('Fetching all recipes...')
          const response = await api.get<RecipeDto[]>('/recipes')
          recipes.value = response.data
          console.log('Fetched recipes:', recipes.value)
        } catch (error: any) {
          console.error('Error fetching recipes:', error)
          // Optionally, display a user-friendly error message
          alert('Failed to fetch recipes. Please try again later.')
        }
      }

      // 2. GET: Fetch recipe by selected ID
      const fetchRecipeById = async () => {
        if (!selectedRecipeId.value) {
          singleRecipeError.value = 'Please select a recipe.'
          singleRecipe.value = null
          return
        }

        console.log('Attempting to fetch recipe by ID...')
        console.log('Selected Recipe ID:', selectedRecipeId.value)

        singleRecipeError.value = null
        singleRecipe.value = null
        isLoading.value = true

        try {
          const response = await api.get<RecipeDto>(`/recipes/${selectedRecipeId.value}`)
          singleRecipe.value = response.data
          console.log('Recipe fetched:', singleRecipe.value)
        } catch (error: any) {
          console.error('Error fetching recipe by ID:', error)
          if (error.response) {
            // Server responded with a status other than 2xx
            singleRecipeError.value = `Error ${error.response.status}: ${error.response.data.message || 'An error occurred while fetching the recipe.'}`
          } else if (error.request) {
            // Request was made but no response received
            singleRecipeError.value = 'No response from the server. Please try again later.'
          } else {
            // Something happened in setting up the request
            singleRecipeError.value = `Error: ${error.message}`
          }
        } finally {
          isLoading.value = false
        }
      }

      // 3. POST: Upsert recipe
      const upsertRecipe = async () => {
        if (!upsertTitle.value || !upsertIngredients.value || !upsertInstructions.value) {
          upsertError.value = 'Please fill in all fields.'
          return
        }

        console.log('Attempting to upsert recipe...')
        console.log('Upsert Data:', {
          id: upsertId.value, // Include ID if updating
          title: upsertTitle.value,
          ingredients: upsertIngredients.value,
          instructions: upsertInstructions.value
        })

        upsertError.value = null
        upsertSuccess.value = null
        isUpserting.value = true

        try {
          const dataToSend: Partial<RecipeDto> & { id?: string } = {
            // Include ID if updating
            ...(upsertId.value ? { id: upsertId.value } : {}),
            title: upsertTitle.value,
            ingredients: upsertIngredients.value,
            instructions: upsertInstructions.value
            // Add description and cuisine if applicable
          }

          const response = await api.post<string>('/recipes/upsert', dataToSend)
          // Assuming the response is a Job ID or Guid, you might want to inform the user
          // Alternatively, you could fetch the updated list after some delay
          await fetchRecipes()
          // Clear form
          upsertId.value = ''
          upsertTitle.value = ''
          upsertIngredients.value = ''
          upsertInstructions.value = ''
          upsertSuccess.value = 'Recipe upserted successfully!'
          console.log('Upsert successful, Response:', response.data)
        } catch (error: any) {
          console.error('Error upserting recipe:', error)
          if (error.response) {
            upsertError.value = error.response.data.message || 'An error occurred while upserting the recipe.'
          } else if (error.request) {
            upsertError.value = 'No response from the server. Please try again later.'
          } else {
            upsertError.value = `Error: ${error.message}`
          }
        } finally {
          isUpserting.value = false
        }
      }

      // 4. DELETE: Delete recipe by selected ID
      const deleteRecipe = async () => {
        if (!selectedDeleteRecipeId.value) {
          deleteError.value = 'Please select a recipe to delete.'
          deleteSuccess.value = null
          return
        }

        console.log('Attempting to delete recipe...')
        console.log('Delete Recipe ID:', selectedDeleteRecipeId.value)

        deleteError.value = null
        deleteSuccess.value = null
        isDeleting.value = true

        try {
          await api.delete(`/recipes/delete/${selectedDeleteRecipeId.value}`)
          // Refresh after deletion
          await fetchRecipes()
          selectedDeleteRecipeId.value = ''
          deleteSuccess.value = 'Recipe deleted successfully!'
          console.log('Deletion successful for ID:', selectedDeleteRecipeId.value)
        } catch (error: any) {
          console.error('Error deleting recipe:', error)
          if (error.response && error.response.status === 404) {
            deleteError.value = `Recipe not found.`
          } else if (error.response) {
            deleteError.value = `Error ${error.response.status}: ${error.response.data.message || 'An error occurred while deleting the recipe.'}`
          } else if (error.request) {
            deleteError.value = 'No response from the server. Please try again later.'
          } else {
            deleteError.value = `Error: ${error.message}`
          }
        } finally {
          isDeleting.value = false
        }
      }

      // 5. SEARCH: Search for a recipe not in the grid
      const searchRecipe = async () => {
        if (!searchTerm.value.trim()) {
          searchError.value = 'Please enter a recipe title to search.'
          searchedRecipe.value = null
          return
        }

        console.log('Attempting to search for recipe...')
        console.log('Search Term:', searchTerm.value)

        searchError.value = null
        searchedRecipe.value = null
        isSearching.value = true

        try {
          const response = await api.get<RecipeDto[]>('/recipes/search', {
            params: { title: searchTerm.value.trim() }
          })
          if (response.data.length > 0) {
            // Assuming the search returns an array of matching recipes
            searchedRecipe.value = response.data[0] // Display the first match
            console.log('Recipe found:', searchedRecipe.value)
          } else {
            searchError.value = 'No recipe found with the provided title.'
            console.log('No recipe found for the search term.')
          }
        } catch (error: any) {
          console.error('Error searching for recipe:', error)
          if (error.response) {
            searchError.value = error.response.data.message || 'An error occurred while searching for the recipe.'
          } else if (error.request) {
            searchError.value = 'No response from the server. Please try again later.'
          } else {
            searchError.value = `Error: ${error.message}`
          }
        } finally {
          isSearching.value = false
        }
      }

      // Load a recipe's details into the upsert form for editing
      const loadRecipeForUpdate = () => {
        if (!singleRecipe.value) {
          upsertError.value = 'No recipe loaded to update.'
          return
        }

        upsertId.value = singleRecipe.value.id
        upsertTitle.value = singleRecipe.value.title
        upsertIngredients.value = singleRecipe.value.ingredients
        upsertInstructions.value = singleRecipe.value.instructions

        upsertSuccess.value = 'Loaded recipe for update. Make your changes and click Upsert.'
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
        selectedRecipeId,
        singleRecipe,
        singleRecipeError,
        fetchRecipeById,
        upsertId,
        upsertTitle,
        upsertIngredients,
        upsertInstructions,
        upsertRecipe,
        upsertError,
        upsertSuccess,
        selectedDeleteRecipeId,
        deleteRecipe,
        deleteError,
        deleteSuccess,
        searchTerm,
        searchRecipe,
        searchedRecipe,
        searchError,
        isLoading,
        isDeleting,
        isUpserting,
        isSearching,
        isValidGUID, // Expose to template for disabling delete button if needed
        loadRecipeForUpdate
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
  .delete-section,
  .search-not-in-grid-section {
    margin: 2rem 0;
  }

  .recipe-select {
    width: 260px;
    padding: 0.5rem;
    margin-right: 0.5rem;
    border: 1px solid #aaa;
    border-radius: 4px;
    appearance: none;
    background-color: #fff;
    background-image: url('data:image/svg+xml;charset=US-ASCII,<svg xmlns="http://www.w3.org/2000/svg" width="10" height="5" viewBox="0 0 10 5"><polygon points="0,0 10,0 5,5" fill="%23999"/></svg>');
    background-repeat: no-repeat;
    background-position: right 0.7rem center;
    background-size: 10px 5px;
  }

  .search-input {
    width: 250px;
    padding: 0.5rem;
    margin-right: 0.5rem;
    border: 1px solid #aaa;
    border-radius: 4px;
  }

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

  .edit-button {
    padding: 0.3rem 0.6rem;
    background-color: #28a745;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 600;
    margin-top: 0.5rem;
  }

    .edit-button:hover {
      background-color: #218838;
    }

  .delete-button {
    background-color: #ff0000;
  }

    .delete-button:hover {
      background-color: #cc0000;
    }

  .searched-recipe-result,
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

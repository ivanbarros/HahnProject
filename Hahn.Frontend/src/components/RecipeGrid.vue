<!-- src/components/RecipeGrid.vue -->

<template>
  <div class="page-container">
    <!-- Page Header -->
    <header class="page-header">
      <h1 class="page-title">Delicious Recipes</h1>
      <p class="page-subtitle">Find and filter Recipes for your next meal!</p>
    </header>

    <!-- Main Content -->
    <main>
      <!-- Filter & Refresh Section (GET all Recipes) -->
      <section class="filter-section">
        <label for="recipeFilter" class="filter-label">Search by Title</label>
        <input id="recipeFilter"
               class="filter-input"
               v-model="filterTerm"
               placeholder="Filter by title..." />
        <button class="filter-button"
                @click="fetchRecipies"
                :disabled="isLoading">
          {{ isLoading ? 'Loading...' : 'Refresh' }}
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
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="recipe in filteredRecipies" :key="recipe.id">
              <td>{{ recipe.title }}</td>
              <td>{{ recipe.ingredients }}</td>
              <td>{{ recipe.instructions }}</td>
              <td>
                <button class="action-button edit-button"
                        @click="loadRecipeForUpdate(recipe)">
                  Edit
                </button>
                <button class="action-button delete-button"
                        @click="deleteRecipeById(recipe.id)">
                  Delete
                </button>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-if="isLoading" class="loading-spinner">
          Loading...
        </div>
        <div v-if="recipies.length === 0 && !isLoading" class="no-recipes">
          No recipes available.
        </div>
      </section>

      <!-- Search By Recipe Selection (GET /api/Recipies/{id}) -->
      <section class="search-id-section">
        <h2>Find Recipe by Selection</h2>
        <select class="recipe-select" v-model="selectedRecipeId">
          <option disabled value="">-- Select a Recipe --</option>
          <option v-for="recipe in recipies"
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
          <button class="edit-button"
                  @click="loadRecipeForUpdate(singleRecipe)">
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

      <!-- Upsert Recipe (POST /api/Recipies/upsert) -->
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
        <input class="upsert-input"
               v-model="upsertMeasure"
               type="text"
               placeholder="Measure" />
        <input class="upsert-input"
               v-model="upsertDescription"
               type="text"
               placeholder="Description" />
        <input class="upsert-input"
               v-model="upsertCuisine"
               type="text"
               placeholder="Cuisine" />
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

      <!-- Delete Recipe (DELETE /api/Recipies/delete/{id}) -->
      <section class="delete-section">
        <h2>Delete Recipe by Selection</h2>
        <select class="recipe-select"
                v-model="selectedDeleteRecipeId">
          <option disabled value="">-- Select a Recipe to Delete --</option>
          <option v-for="recipe in recipies"
                  :key="recipe.id"
                  :value="recipe.id">
            {{ recipe.title }}
          </option>
        </select>
        <button class="action-button delete-button"
                @click="deleteRecipeById(recipe.id)"
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
  import { defineComponent, onMounted } from 'vue';
  import { useRecipeGrid } from '@/composables/useRecipeGrid';
  import type { FoodRecipeDto } from '@/types/FoodRecipeDto';

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      const recipeGrid = useRecipeGrid();

      // Initialize data on component mount
      onMounted(() => {
        recipeGrid.fetchRecipies();
      });

      // Method to delete a recipe by ID with confirmation
      const deleteRecipeById = (id: string) => {
        if (confirm('Are you sure you want to delete this recipe?')) {
          recipeGrid.selectedDeleteRecipeId.value = id;
          recipeGrid.deleteRecipe();
        }
      };

      // Modify loadRecipeForUpdate to accept a recipe object
      const loadRecipeForUpdate = (recipe: FoodRecipeDto) => {
        recipeGrid.loadRecipeForUpdate(recipe);
        recipeGrid.singleRecipe.value = recipe;
      };

      return {
        ...recipeGrid,
        deleteRecipeById,
        loadRecipeForUpdate,
      };
    },
  });
</script>

<!-- Import the separated styles -->
<style scoped src="./RecipeGridStyles.css"></style>

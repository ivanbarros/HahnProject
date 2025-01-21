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
      <!-- Filter & Refresh Section -->
      <section class="filter-section">
        <label for="recipeFilter" class="filter-label">Search by Title</label>
        <input id="recipeFilter"
               class="filter-input"
               v-model="filterTerm"
               placeholder="Filter by title..." />
        <button class="filter-button"
                @click="fetchRecipes"
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
            <tr v-for="recipe in filteredRecipes" :key="recipe.id">
              <td>{{ recipe.title }}</td>
              <td>{{ recipe.ingredients }}</td>
              <td>{{ recipe.instructions }}</td>
              <td>
                <router-link :to="`/recipes/${recipe.id}`"
                             class="action-button view-button">
                  View
                </router-link>
                <router-link :to="`/upsert/${recipe.id}`"
                             class="action-button edit-button">
                  Edit
                </router-link>
                <button class="action-button delete-button"
                        @click="confirmDelete(recipe.id)"
                        :disabled="isDeleting">
                  {{ isDeleting ? 'Deleting...' : 'Delete' }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-if="isLoading" class="loading-spinner">
          Loading...
        </div>
        <div v-if="recipes.length === 0 && !isLoading" class="no-recipes">
          No recipes available.
        </div>
      </section>

      <!-- Additional Sections... -->
    </main>
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, watch } from 'vue';
  import { useRecipes } from '@/composables/useRecipeGrid';

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      const recipeGrid = useRecipes();

      // Initialize data on component mount
      onMounted(() => {
        recipeGrid.fetchRecipes();
      });

      // Watch for changes in filteredRecipes
      watch(
        () => recipeGrid.filteredRecipes,
        (newVal) => {
          console.log('Filtered Recipes:', newVal); // Debugging Line
        }
      );

      // Method to delete a recipe by ID with confirmation
      const confirmDelete = (id: string) => {
        if (confirm('Are you sure you want to delete this recipe?')) {
          recipeGrid.deleteRecipe(id);
        }
      };

      return {
        ...recipeGrid,
        confirmDelete,
      };
    },
  });
</script>

<style scoped src="./RecipeGridStyles.css"></style>

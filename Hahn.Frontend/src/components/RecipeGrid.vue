<!-- src/components/RecipeGrid.vue -->

<template>
  <div class="page-container">
    <!-- Page Header -->
    <header class="page-header">
      <h1 class="page-title">Delicious Recipies</h1>
      <p class="page-subtitle">Find and filter Recipies for your next meal!</p>
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
                @click="fetchRecipies"
                :disabled="isLoading">
          {{ isLoading ? 'Loading...' : 'Refresh' }}
        </button>
      </section>

      <!-- Display Error Message -->
      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <!-- Recipies Table -->
      <section class="table-section">
        <table class="recipe-table">
          <thead>
            <tr>
              <th scope="col">Title</th>
              <th scope="col">Instructions</th>
              <th scope="col">Ingredients</th>
              <th scope="col">Description</th>
              <th scope="col">Cuisine</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="recipe in filteredRecipies" :key="recipe.id">
              <td>{{ recipe.strMeal }}</td>
              <td>{{ recipe.strInstructions }}</td>
              <td>
                <ul>
                  <li v-for="(item, index) in recipe.ingredients" :key="index">
                    {{ item.measure ? `${item.measure} ` : '' }}{{ item.ingredient }}
                  </li>
                </ul>
              </td>
              <td>{{ recipe.description || 'N/A' }}</td>
              <td>{{ recipe.cuisine || 'N/A' }}</td>
              <td>
                <router-link :to="`/recipies/${recipe.id}`"
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
        <div v-if="filteredRecipies.length === 0 && !isLoading" class="no-recipies">
          No recipies available.
        </div>
      </section>
    </main>
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, watch } from 'vue';
  import { useRecipies } from '@/composables/useRecipeGrid';

  export default defineComponent({
    name: 'RecipeGrid',
    setup() {
      const recipeGrid = useRecipies();

      // Initialize data on component mount
      onMounted(() => {
        console.log('Component mounted. Fetching recipies...');
        recipeGrid.fetchRecipies();
      });

      // Watch for changes in recipies to debug data fetching
      watch(
        () => recipeGrid.recipies,
        (newVal) => {
          console.log('Recipies updated:', newVal);
        }
      );

      // Watch for changes in filteredRecipies to debug filtering
      watch(
        () => recipeGrid.filteredRecipies,
        (newVal) => {
          console.log('Filtered Recipies:', newVal);
        }
      );

      // Watch for changes in error to log errors
      watch(
        () => recipeGrid.error,
        (newVal) => {
          if (newVal) {
            console.error('Error:', newVal);
          }
        }
      );

      // Method to delete a recipe by ID with confirmation
      const confirmDelete = (id: string) => {
        if (confirm('Are you sure you want to delete this recipe?')) {
          console.log(`Deleting recipe with ID: ${id}`);
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

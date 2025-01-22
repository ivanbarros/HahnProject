<!-- src/components/RecipeDetail.vue -->

<template>
  <div class="recipe-detail-container" v-if="recipe">
    <h1>{{ recipe.title }}</h1>
    <p><strong>Ingredients:</strong> {{ recipe.ingredients }}</p>
    <p><strong>Instructions:</strong> {{ recipe.instructions }}</p>
    <p v-if="recipe.measure"><strong>Measure:</strong> {{ recipe.measure }}</p>
    <p v-if="recipe.description"><strong>Description:</strong> {{ recipe.description }}</p>
    <p v-if="recipe.cuisine"><strong>Cuisine:</strong> {{ recipe.cuisine }}</p>

    <div class="actions">
      <router-link :to="`/upsert/${recipe.id}`" class="action-button edit-button">
        Edit
      </router-link>
      <button @click="confirmDelete(recipe.id)"
              :disabled="isDeleting"
              class="action-button delete-button">
        {{ isDeleting ? 'Deleting...' : 'Delete' }}
      </button>
      <router-link to="/recipies" class="action-button back-button">
        Back to List
      </router-link>
    </div>
  </div>

  <div v-else-if="!isLoading && !error" class="not-found">
    <p>Recipe not found.</p>
    <router-link to="/recipies" class="action-button back-button">
      Back to List
    </router-link>
  </div>

  <div v-if="isLoading" class="loading">
    Loading...
  </div>

  <div v-if="error" class="error-message">
    {{ error }}
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, ref } from 'vue';
  import { useRecipies } from '@/composables/useRecipeGrid';
  import { useRoute, useRouter } from 'vue-router';
  import type { FoodRecipeDto } from '@/types/FoodRecipeDto';

  export default defineComponent({
    name: 'RecipeDetail',
    setup() {
      const route = useRoute();
      const router = useRouter();
      const { fetchRecipeById, deleteRecipe } = useRecipies();

      const recipe = ref<FoodRecipeDto | null>(null);
      const isLoading = ref<boolean>(false);
      const isDeleting = ref<boolean>(false);
      const error = ref<string | null>(null);

      onMounted(async () => {
        const id = route.params.id as string;
        if (id) {
          isLoading.value = true;
          try {
            const fetchedRecipe = await fetchRecipeById(id);
            if (fetchedRecipe) {
              recipe.value = fetchedRecipe;
            } else {
              error.value = 'Recipe not found.';
            }
          } catch (err) {
            error.value = 'Failed to load the recipe.';
          } finally {
            isLoading.value = false;
          }
        } else {
          error.value = 'Invalid recipe ID.';
        }
      });

      const confirmDelete = async (id: string) => {
        if (confirm('Are you sure you want to delete this recipe?')) {
          isDeleting.value = true;
          try {
            await deleteRecipe(id);
            router.push('/recipies');
          } catch (err) {
            error.value = 'Failed to delete the recipe.';
          } finally {
            isDeleting.value = false;
          }
        }
      };

      return {
        recipe,
        isLoading,
        isDeleting,
        error,
        confirmDelete,
      };
    },
  });
</script>

<style scoped>
  .recipe-detail-container {
    padding: 20px;
    max-width: 800px;
    margin: 0 auto;
  }

  .actions {
    margin-top: 20px;
    display: flex;
    gap: 10px;
  }

  .action-button {
    padding: 8px 16px;
    font-size: 14px;
    cursor: pointer;
    text-decoration: none;
    border: none;
    color: white;
  }

  .edit-button {
    background-color: #2196f3;
  }

  .delete-button {
    background-color: #f44336;
  }

  .back-button {
    background-color: #9e9e9e;
  }

  .loading {
    text-align: center;
    font-size: 18px;
    color: #555;
  }

  .not-found {
    text-align: center;
    font-size: 18px;
    color: #555;
  }

  .error-message {
    text-align: center;
    color: red;
    font-size: 16px;
    margin-top: 20px;
  }
</style>

<!-- src/components/RecipeForm.vue -->

<template>
  <div class="recipe-form-container">
    <h1>{{ isEditMode ? 'Edit Recipe' : 'Create Recipe' }}</h1>

    <form @submit.prevent="handleSubmit" class="recipe-form">
      <div class="form-group">
        <label for="title">Title<span class="required">*</span></label>
        <input id="title"
               v-model="title"
               type="text"
               required
               placeholder="Enter recipe title" />
      </div>

      <div class="form-group">
        <label for="ingredients">Ingredients<span class="required">*</span></label>
        <textarea id="ingredients"
                  v-model="ingredients"
                  required
                  placeholder="Enter ingredients"></textarea>
      </div>

      <div class="form-group">
        <label for="instructions">Instructions<span class="required">*</span></label>
        <textarea id="instructions"
                  v-model="instructions"
                  required
                  placeholder="Enter instructions"></textarea>
      </div>

      <div class="form-group">
        <label for="measure">Measure</label>
        <input id="measure"
               v-model="measure"
               type="text"
               placeholder="Enter measure" />
      </div>

      <div class="form-group">
        <label for="description">Description</label>
        <textarea id="description"
                  v-model="description"
                  placeholder="Enter description"></textarea>
      </div>

      <div class="form-group">
        <label for="cuisine">Cuisine</label>
        <input id="cuisine"
               v-model="cuisine"
               type="text"
               placeholder="Enter cuisine" />
      </div>

      <button type="submit"
              :disabled="!isFormValid || isSubmitting"
              class="submit-button">
        {{ isSubmitting ? 'Submitting...' : isEditMode ? 'Update' : 'Create' }}
      </button>
    </form>

    <div v-if="error" class="error-message">
      {{ error }}
    </div>
    <div v-if="successMessage" class="success-message">
      {{ successMessage }}
    </div>

    <router-link to="/recipes" class="action-button back-button">
      Back to List
    </router-link>
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, ref, computed } from 'vue';
  import { useRecipes } from '@/composables/useRecipeGrid';
  import { useRoute, useRouter } from 'vue-router';
  import type { FoodRecipeDto } from '@/types/FoodRecipeDto';

  export default defineComponent({
    name: 'RecipeForm',
    setup() {
      const route = useRoute();
      const router = useRouter();
      const { fetchRecipeById, upsertRecipe } = useRecipes();

      // Form Fields
      const title = ref<string>('');
      const ingredients = ref<string>('');
      const instructions = ref<string>('');
      const measure = ref<string>('');
      const description = ref<string>('');
      const cuisine = ref<string>('');

      const isEditMode = ref<boolean>(false);
      const recipeId = ref<string | null>(null);

      const isSubmitting = ref<boolean>(false);
      const error = ref<string | null>(null);
      const successMessage = ref<string | null>(null);

      // Computed property to check form validity
      const isFormValid = computed(() => {
        return (
          title.value.trim() !== '' &&
          ingredients.value.trim() !== '' &&
          instructions.value.trim() !== ''
        );
      });

      onMounted(async () => {
        const id = route.params.id as string | undefined;
        if (id) {
          isEditMode.value = true;
          recipeId.value = id;
          try {
            const recipe = await fetchRecipeById(id);
            if (recipe) {
              title.value = recipe.title;
              ingredients.value = recipe.ingredients;
              instructions.value = recipe.instructions;
              measure.value = recipe.measure || '';
              description.value = recipe.description || '';
              cuisine.value = recipe.cuisine || '';
            } else {
              error.value = 'Recipe not found.';
            }
          } catch (err) {
            error.value = 'Failed to load the recipe.';
          }
        }
      });

      const handleSubmit = async () => {
        if (!isFormValid.value) {
          error.value = 'Please fill in all required fields.';
          return;
        }

        isSubmitting.value = true;
        error.value = null;
        successMessage.value = null;

        const recipeData: Partial<FoodRecipeDto> = {
          title: title.value,
          ingredients: ingredients.value,
          instructions: instructions.value,
          measure: measure.value || undefined,
          description: description.value || undefined,
          cuisine: cuisine.value || undefined,
        };

        if (isEditMode.value && recipeId.value) {
          recipeData.id = recipeId.value;
        }

        try {
          const result = await upsertRecipe(recipeData);
          if (result) {
            successMessage.value = isEditMode.value
              ? 'Recipe updated successfully.'
              : 'Recipe created successfully.';
            // Optionally, redirect to the recipe detail or list after a delay
            setTimeout(() => {
              router.push('/recipes');
            }, 1500);
          }
        } catch (err) {
          error.value = 'An error occurred while submitting the form.';
        } finally {
          isSubmitting.value = false;
        }
      };

      return {
        title,
        ingredients,
        instructions,
        measure,
        description,
        cuisine,
        isEditMode,
        isSubmitting,
        error,
        successMessage,
        isFormValid,
        handleSubmit,
      };
    },
  });
</script>

<style scoped>
  .recipe-form-container {
    padding: 20px;
    max-width: 600px;
    margin: 0 auto;
  }

  .recipe-form {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .form-group {
    display: flex;
    flex-direction: column;
  }

    .form-group label {
      font-weight: bold;
      margin-bottom: 5px;
    }

  .required {
    color: red;
  }

  .form-group input,
  .form-group textarea {
    padding: 8px;
    font-size: 16px;
  }

  .submit-button {
    padding: 10px 20px;
    font-size: 16px;
    background-color: #4caf50;
    color: white;
    border: none;
    cursor: pointer;
  }

    .submit-button:disabled {
      background-color: #a5d6a7;
      cursor: not-allowed;
    }

  .back-button {
    margin-top: 20px;
    padding: 8px 16px;
    font-size: 16px;
    background-color: #9e9e9e;
    color: white;
    border: none;
    text-decoration: none;
    display: inline-block;
  }

  .error-message {
    color: red;
    margin-top: 10px;
  }

  .success-message {
    color: green;
    margin-top: 10px;
  }
</style>

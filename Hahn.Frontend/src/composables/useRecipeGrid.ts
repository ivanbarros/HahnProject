// src/composables/useRecipeGrid.ts

import { ref, computed } from 'vue';
import api from '@/api';
import type { FoodRecipeDto } from '@/types/FoodRecipeDto';
import type { UpsertFoodRecipeDto } from '@/types/UpsertFoodRecipeDto';

export function useRecipeGrid() {
  // Reactive state variables
  const recipies = ref<FoodRecipeDto[]>([]);
  const filterTerm = ref<string>('');
  const selectedRecipeId = ref<string>('');
  const singleRecipe = ref<FoodRecipeDto | null>(null);
  const singleRecipeError = ref<string | null>(null);
  const searchTerm = ref<string>('');
  const searchedRecipe = ref<FoodRecipeDto | null>(null);
  const searchError = ref<string | null>(null);
  const upsertId = ref<string>(''); // For Update
  const upsertTitle = ref<string>('');
  const upsertIngredients = ref<string>('');
  const upsertInstructions = ref<string>('');
  const upsertMeasure = ref<string>('');
  const upsertDescription = ref<string>('');
  const upsertCuisine = ref<string>('');
  const upsertError = ref<string | null>(null);
  const upsertSuccess = ref<string | null>(null);
  const selectedDeleteRecipeId = ref<string>('');
  const deleteError = ref<string | null>(null);
  const deleteSuccess = ref<string | null>(null);
  const isLoading = ref<boolean>(false);
  const isSearching = ref<boolean>(false);
  const isUpserting = ref<boolean>(false);
  const isDeleting = ref<boolean>(false); // Corrected line

  // Computed property for filtered recipes
  const filteredRecipies = computed(() => {
    if (!filterTerm.value) {
      return recipies.value;
    }
    return recipies.value.filter((recipe) =>
      recipe.title.toLowerCase().includes(filterTerm.value.toLowerCase())
    );
  });

  // Fetch all recipes with pagination
  const fetchRecipies = async (pageNumber = 1, pageSize = 20) => {
    isLoading.value = true;
    recipies.value = [];
    try {
      const response = await api.get<FoodRecipeDto[]>('/Recipies', {
        params: {
          pageNumber,
          pageSize,
        },
      });
      recipies.value = response.data;
    } catch (error: any) {
      console.error('Error fetching Recipies:', error);
      // Optionally, set a global error state
    } finally {
      isLoading.value = false;
    }
  };

  // Fetch a single recipe by ID
  const fetchRecipeById = async () => {
    if (!selectedRecipeId.value) {
      return;
    }
    singleRecipe.value = null;
    singleRecipeError.value = null;
    isLoading.value = true;
    try {
      const response = await api.get<FoodRecipeDto>(`/Recipies/${selectedRecipeId.value}`);
      singleRecipe.value = response.data;
    } catch (error: any) {
      console.error('Error fetching Recipe by ID:', error);
      singleRecipeError.value = error.response?.data || 'An error occurred while fetching the recipe.';
    } finally {
      isLoading.value = false;
    }
  };

  // Search for a recipe by title
  const searchRecipe = async () => {
    if (!searchTerm.value.trim()) {
      return;
    }
    isSearching.value = true;
    searchedRecipe.value = null;
    searchError.value = null;
    try {
      const response = await api.get<FoodRecipeDto[]>('/Recipies/search', {
        params: {
          title: searchTerm.value,
        },
      });
      if (response.data.length > 0) {
        // Assuming the first match is the desired one
        searchedRecipe.value = response.data[0];
      } else {
        searchError.value = 'No recipes found matching the search term.';
      }
    } catch (error: any) {
      console.error('Error searching recipes:', error);
      searchError.value = error.response?.data || 'An error occurred while searching for recipes.';
    } finally {
      isSearching.value = false;
    }
  };

  // Upsert (Create/Update) a recipe
  const upsertRecipe = async () => {
    if (!upsertTitle.value || !upsertIngredients.value || !upsertInstructions.value) {
      upsertError.value = 'Title, Ingredients, and Instructions are required.';
      return;
    }

    isUpserting.value = true;
    upsertError.value = null;
    upsertSuccess.value = null;

    const upsertDto: UpsertFoodRecipeDto = {
      id: upsertId.value ? upsertId.value : null, // Null for creation
      title: upsertTitle.value,
      ingredients: upsertIngredients.value,
      instructions: upsertInstructions.value,
      measure: upsertMeasure.value || undefined,
      description: upsertDescription.value || undefined,
      cuisine: upsertCuisine.value || undefined,
    };

    try {
      const response = await api.post<FoodRecipeDto>('/Recipies/upsert', upsertDto);
      upsertSuccess.value = `Recipe "${response.data.title}" has been successfully upserted.`;
      // Refresh the recipes list
      await fetchRecipies();
      // Reset the form
      resetUpsertForm();
    } catch (error: any) {
      console.error('Error upserting recipe:', error);
      upsertError.value = error.response?.data || 'An error occurred while upserting the recipe.';
    } finally {
      isUpserting.value = false;
    }
  };

  // Load recipe data into the upsert form for updating
  const loadRecipeForUpdate = (recipe: FoodRecipeDto) => {
    upsertId.value = recipe.id;
    upsertTitle.value = recipe.title;
    upsertIngredients.value = recipe.ingredients;
    upsertInstructions.value = recipe.instructions;
    upsertMeasure.value = recipe.measure || '';
    upsertDescription.value = recipe.description || '';
    upsertCuisine.value = recipe.cuisine || '';
  };

  // Reset the upsert form
  const resetUpsertForm = () => {
    upsertId.value = '';
    upsertTitle.value = '';
    upsertIngredients.value = '';
    upsertInstructions.value = '';
    upsertMeasure.value = '';
    upsertDescription.value = '';
    upsertCuisine.value = '';
  };

  // Delete a recipe by ID
  const deleteRecipe = async () => {
    if (!selectedDeleteRecipeId.value) {
      return;
    }
    isDeleting.value = true;
    deleteError.value = null;
    deleteSuccess.value = null;

    try {
      await api.delete(`/Recipies/delete/${selectedDeleteRecipeId.value}`);
      deleteSuccess.value = 'Recipe has been successfully deleted.';
      // Refresh the recipes list
      await fetchRecipies();
      // Reset selection
      selectedDeleteRecipeId.value = '';
    } catch (error: any) {
      console.error('Error deleting recipe:', error);
      deleteError.value = error.response?.data || 'An error occurred while deleting the recipe.';
    } finally {
      isDeleting.value = false;
    }
  };

  return {
    // State variables
    recipies,
    filterTerm,
    selectedRecipeId,
    singleRecipe,
    singleRecipeError,
    searchTerm,
    searchedRecipe,
    searchError,
    upsertId,
    upsertTitle,
    upsertIngredients,
    upsertInstructions,
    upsertMeasure,
    upsertDescription,
    upsertCuisine,
    upsertError,
    upsertSuccess,
    selectedDeleteRecipeId,
    deleteError,
    deleteSuccess,
    isLoading,
    isSearching,
    isUpserting,
    isDeleting,
    // Computed properties
    filteredRecipies,
    // Methods
    fetchRecipies,
    fetchRecipeById,
    searchRecipe,
    upsertRecipe,
    loadRecipeForUpdate,
    resetUpsertForm,
    deleteRecipe,
  };
}

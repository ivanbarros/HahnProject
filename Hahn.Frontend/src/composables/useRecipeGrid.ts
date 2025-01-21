import { ref, computed } from 'vue';
import api from '../services/api';
import type { FoodRecipeDto } from '../types/FoodRecipeDto';
import { useToast } from 'vue-toastification';

export function useRecipes() {
  // State Variables
  const recipes = ref<FoodRecipeDto[]>([]);
  const filterTerm = ref<string>('');
  const isLoading = ref<boolean>(false);
  const isUpserting = ref<boolean>(false);
  const isDeleting = ref<boolean>(false);

  const toast = useToast();

  // Computed Properties
  const filteredRecipes = computed(() => {
    if (!filterTerm.value) {
      return recipes.value;
    }
    return recipes.value.filter((recipe) =>
      recipe.title.toLowerCase().includes(filterTerm.value.toLowerCase())
    );
  });

  // Fetch All Recipes
  const fetchRecipes = async () => {
    isLoading.value = true;
    try {
      const response = await api.get<FoodRecipeDto[]>('/Recipies');
      recipes.value = response.data;
      console.log('Fetched Recipes:', recipes.value); // Debugging Line
    } catch (error: any) {
      console.error('Error fetching recipes:', error);
      toast.error('Failed to fetch recipes.');
    } finally {
      isLoading.value = false;
    }
  };

  // Delete Recipe
  const deleteRecipe = async (id: string) => {
    isDeleting.value = true;
    try {
      await api.delete(`/Recipies/delete/${id}`);
      // Remove from local list
      recipes.value = recipes.value.filter((recipe) => recipe.id !== id);
      toast.success('Recipe deleted successfully.');
    } catch (error: any) {
      console.error('Error deleting recipe:', error);
      toast.error('Failed to delete the recipe.');
    } finally {
      isDeleting.value = false;
    }
  };

  return {
    recipes,
    filterTerm,
    isLoading,
    isUpserting,
    isDeleting,
    filteredRecipes,
    fetchRecipes,
    deleteRecipe,
  };
}

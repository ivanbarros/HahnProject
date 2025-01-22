import { ref, computed } from 'vue';
import type { FoodRecipeDto } from '../types/FoodRecipeDto';
import { useToast } from 'vue-toastification';
import axiosInstance from '@/services/axiosInstance';

export function useRecipies() {
  // State Variables
  const recipies = ref<FoodRecipeDto[]>([]);
  const filterTerm = ref<string>('');
  const isLoading = ref<boolean>(false);
  const isDeleting = ref<boolean>(false);
  const error = ref<string | null>(null);

  const toast = useToast();

  // Computed Properties
  const filteredRecipies = computed(() => {
    if (!filterTerm.value) {
      return recipies.value;
    }
    return recipies.value.filter(recipe =>
      recipe.strMeal.toLowerCase().includes(filterTerm.value.toLowerCase())
    );
  });

  // Function to Map Ingredients (if necessary)
  const mapIngredients = (item: any) => {
    const ingredients = [];
    for (let i = 1; i <= 20; i++) { // Assuming a maximum of 20 ingredients
      const ingredient = item[`strIngredient${i}`];
      const measure = item[`strMeasure${i}`];
      if (ingredient && ingredient.trim() !== "") {
        ingredients.push({ ingredient, measure });
      }
    }
    return ingredients;
  };


  const fetchRecipies = async () => {
    isLoading.value = true;
    error.value = null;
    try {
      console.log('Fetching recipies from /api/Recipies...');
      const response = await axiosInstance.get<FoodRecipeDto[]>('Recipies');

      console.log('Data from Ivan API:', response.data);

      recipies.value = response.data;
    } catch (err: any) {
      console.error('Error fetching recipies:', err);
      toast.error('Failed to fetch recipies.');
      error.value = 'Unable to load recipies. Please try again later.';
    } finally {
      isLoading.value = false;
      console.log('Loading completed.');
    }
  };

  // Delete Recipe using Axios
  const deleteRecipe = async (id: string) => {
    isDeleting.value = true;
    try {
      console.log(`Deleting recipe with ID: ${id}`);
      const response = await axiosInstance.delete(`Recipies/delete/${id}`);

      console.log('Recipe deleted successfully.', response.data);

      // Remove the deleted recipe from the local list
      recipies.value = recipies.value.filter(recipe => recipe.id !== id);
      toast.success('Recipe deleted successfully.');
    } catch (err: any) {
      console.error('Error deleting recipe:', err);
      toast.error('Failed to delete the recipe.');
    } finally {
      isDeleting.value = false;
      console.log('Deletion process completed.');
    }
  };

  return {
    recipies,
    filterTerm,
    isLoading,
    isDeleting,
    filteredRecipies,
    fetchRecipies,
    deleteRecipe,
    error,
  };
}

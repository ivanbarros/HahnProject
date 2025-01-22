// src/types/FoodRecipeDto.ts

export interface Ingredient {
  ingredient: string;
  measure: string;
}

export interface FoodRecipeDto {
  strMeal: string;
  strInstructions: string;
  ingredients: Ingredient[];
  description?: string; // Optional fields
  cuisine?: string;
  jobId?: string | null;
  id: string;
}

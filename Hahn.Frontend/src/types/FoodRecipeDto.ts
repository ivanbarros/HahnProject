// src/types/FoodRecipeDto.ts

export interface FoodRecipeDto {
  id: string; // Assuming GUID is represented as a string
  title: string;
  ingredients: string;
  instructions: string;
  measure?: string;
  description?: string;
  cuisine?: string;
}

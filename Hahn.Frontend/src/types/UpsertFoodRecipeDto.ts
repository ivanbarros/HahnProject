
export interface UpsertFoodRecipeDto {
  id?: string | null;
  title: string;
  ingredients: string;
  instructions: string;
  measure?: string;
  description?: string;
  cuisine?: string;
}

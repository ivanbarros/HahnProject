// src/router/index.ts

import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import RecipeGrid from '../components/RecipeGrid.vue';
import RecipeDetail from '../components/RecipeDetail.vue';
import RecipeForm from '../components/RecipeForm.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: RecipeGrid,
  },
  {
    path: '/recipes',
    name: 'RecipeGrid',
    component: RecipeGrid,
  },
  {
    path: '/recipes/:id',
    name: 'RecipeDetail',
    component: RecipeDetail,
    props: true,
  },
  {
    path: '/upsert/:id?',
    name: 'RecipeUpsert',
    component: RecipeForm,
    props: true,
  },
  // Add more routes as needed
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;

import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue'; // Single import for Vue plugin
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()], // Corrected plugins array
  server: {
    port: 49799, // Server runs on port 49799
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'), // Alias '@' to '/src'
    },
  },
});

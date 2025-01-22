import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';
//import { createHtmlPlugin } from 'vite-plugin-html'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), vueDevTools(),
   /* createHtmlPlugin({})*/
  ],
  
  server: {
    port: 49799,
    proxy: {
      '/ api': {
        target: 'https://localhost:7105',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/api/, ''),
      }
    }
  },

  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'), // Alias '@' to '/src'
    },
  },
});

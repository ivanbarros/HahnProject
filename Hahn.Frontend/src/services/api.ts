// src/services/api.ts

import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7105/api',
  timeout: 30000, // 30 seconds
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor (optional)
api.interceptors.request.use(
  (config) => {
    // Add authentication tokens if required
    const token = localStorage.getItem('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor (optional)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle global errors
    if (error.response && error.response.status === 401) {
      // Handle unauthorized access, e.g., redirect to login
    }
    return Promise.reject(error);
  }
);

export default api;

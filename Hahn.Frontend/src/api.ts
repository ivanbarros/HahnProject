// src/api.ts
import axios from 'axios'

// Create an Axios instance with a base URL
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7105/api', // Use environment variables for flexibility
  timeout: 5000, // Optional: Set a timeout for requests
  headers: {
    'Content-Type': 'application/json',
  },
})

// Optional: Add interceptors for logging or authentication
api.interceptors.request.use(config => {
  console.log('Starting Request', config)
  return config
})

api.interceptors.response.use(response => {
  console.log('Response:', response)
  return response
}, error => {
  console.error('API Error:', error)
  return Promise.reject(error)
})

export default api

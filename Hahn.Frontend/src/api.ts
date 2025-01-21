import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7105/api',
  timeout: 5000,
  headers: {
    'Content-Type': 'application/json',
  },
})


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

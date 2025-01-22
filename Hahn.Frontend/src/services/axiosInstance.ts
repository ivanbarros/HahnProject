// src/services/axiosInstance.ts

import axios from 'axios';



const axiosInstance = axios.create({
  baseURL: 'https://localhost:7105/api/',
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});
console.log('API Base URL:', axios);

export default axiosInstance;

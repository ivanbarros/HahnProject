import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7105/api',
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});


api.interceptors.request.use(
  (config) => {

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
   
    return Promise.reject(error);
  }
);

export default api;

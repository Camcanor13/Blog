import { createApp } from 'vue';
import App from '../App.vue';
import axios from 'axios';
import router from '../router.js'; // Importa el archivo de rutas

// Configuración global de Axios
//axios.defaults.baseURL = 'https://localhost:7079';
axios.defaults.baseURL = "https://paths-socks-browser-shepherd.trycloudflare.com"

const app = createApp(App);

// Agregar Axios a la instancia global de Vue
app.config.globalProperties.$axios = axios;

// Usar el router en la aplicación Vue
app.use(router);

app.mount('#app');

import axios from "axios";

const $host = axios.create({
    baseURL: import.meta.env.VITE_BASE_API_URL,
    validateStatus: status => true
})

const $authHost = axios.create({
    baseURL: import.meta.env.VITE_BASE_API_URL,
    validateStatus: status => true
})

function authInterceptor(config) {
    let token = localStorage.getItem("accessToken");
    config.headers.Authorization = `Bearer ${token}`

    return config
}

$authHost.interceptors.request.use(authInterceptor)

export {
    $host,
    $authHost
}
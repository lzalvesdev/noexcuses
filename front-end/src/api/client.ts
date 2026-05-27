import { config } from "@/config";
import axios from "axios";

const API_URL = config.apiUrl;
export const client = axios.create({ baseURL: API_URL });

let isRefreshing = false;
let queue: Array<{ resolve: (token: string) => void; reject: (err: unknown) => void }> = [];

function processQueue(error: unknown, token: string | null = null) {
    queue.forEach(({ resolve, reject }) => error ? reject(error) : resolve(token!));
    queue = [];
}

client.interceptors.request.use((requestConfig) => {
    const token = localStorage.getItem('noExcuses_token');
    if (token) {
        requestConfig.headers.Authorization = `Bearer ${token}`;
    }
    return requestConfig;
});

client.interceptors.response.use(
    (response) => response,
    async (error) => {
        const original = error.config;
        const isAuthEndpoint = original.url?.includes('/Auth/login')
            || original.url?.includes('/Auth/refresh')
            || original.url?.includes('/Auth/register');

        if (error.response?.status === 401 && !original._retry && !isAuthEndpoint) {
            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    queue.push({ resolve, reject });
                }).then(token => {
                    original.headers.Authorization = `Bearer ${token}`;
                    return client(original);
                });
            }

            original._retry = true;
            isRefreshing = true;

            try {
                const refreshToken = localStorage.getItem('noExcuses_refresh');
                const { data } = await axios.post(`${API_URL}/Auth/refresh`, { refreshToken });

                localStorage.setItem('noExcuses_token', data.token);
                localStorage.setItem('noExcuses_refresh', data.refreshToken);

                processQueue(null, data.token);
                original.headers.Authorization = `Bearer ${data.token}`;
                return client(original);
            } catch (err) {
                processQueue(err);
                localStorage.removeItem('noExcuses_token');
                localStorage.removeItem('noExcuses_refresh');
                window.dispatchEvent(new Event('auth:logout'));
                return Promise.reject(new Error('session_expired'));
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    }
);
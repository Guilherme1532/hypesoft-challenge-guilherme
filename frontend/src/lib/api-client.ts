import axios, {
  AxiosError,
  AxiosHeaders,
  type InternalAxiosRequestConfig,
} from "axios";
import { API_BASE_URL } from "@/lib/env";
import { getToken, login, updateToken } from "@/services/auth";

type RetryableRequestConfig = InternalAxiosRequestConfig & { _retry?: boolean };

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
});

apiClient.interceptors.request.use(async (config) => {
  try {
    await updateToken();
  } catch {
    // Token update may fail when session is already invalid.
  }

  const token = getToken();
  if (!token) {
    return config;
  }

  if (!(config.headers instanceof AxiosHeaders)) {
    config.headers = AxiosHeaders.from(config.headers);
  }

  config.headers.set("Authorization", `Bearer ${token}`);
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const status = error.response?.status;
    const originalRequest = error.config as RetryableRequestConfig | undefined;

    if (status === 401 && originalRequest && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        await updateToken();
        const token = getToken();

        if (token) {
          if (!(originalRequest.headers instanceof AxiosHeaders)) {
            originalRequest.headers = AxiosHeaders.from(originalRequest.headers);
          }
          originalRequest.headers.set("Authorization", `Bearer ${token}`);
          return apiClient(originalRequest);
        }
      } catch {
        // If refresh fails, we redirect to login below.
      }
    }

    if (status === 401) {
      void login();
    }

    return Promise.reject(error);
  }
);

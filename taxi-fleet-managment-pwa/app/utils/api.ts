"use server";

import axios from "axios";
import { cookies } from "next/headers";
import TokenResponse from "../interfaces/TokenResponse";

const api = axios.create({
  baseURL: `${process.env.NEXT_PUBLIC_API_URL}/api`,
});

api.interceptors.request.use(
  async (config) => {
    const cookieStore = await cookies();
    const tokenString = cookieStore.get("userToken")?.value;

    if (tokenString) {
      const { accessToken } = JSON.parse(tokenString);
      config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
  },
  (error) => Promise.reject(error),
);

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    const cookieStore = await cookies();

    if (error.response?.status === 401 && !originalRequest?._retry) {
      originalRequest._retry = true;

      try {
        const savedTokenString = cookieStore.get("userToken")?.value;

        if (!savedTokenString) {
          throw new Error("No refresh token available");
        }

        const { refreshToken }: TokenResponse = JSON.parse(savedTokenString);

        const refreshResponse = await axios.post(
          `${process.env.NEXT_PUBLIC_API_URL}/api/auth/refresh`,
          {
            refreshToken,
          },
        );

        const { accessToken: newAccess, refreshToken: newRefresh } =
          refreshResponse.data;

        cookieStore.set(
          "userToken",
          JSON.stringify({
            accessToken: newAccess,
            refreshToken: newRefresh,
          }),
          {
            httpOnly: true,
            secure: process.env.NODE_ENV === "production",
            sameSite: "strict",
            path: "/",
            maxAge: 30 * 24 * 60 * 60,
          },
        );

        api.defaults.headers.common.Authorization = `Bearer ${newAccess}`;
        originalRequest.headers = {
          ...originalRequest.headers,
          Authorization: `Bearer ${newAccess}`,
        };

        return api(originalRequest);
      } catch (e) {
        cookieStore.delete("userToken");
        return Promise.reject(e);
      }
    }

    return Promise.reject(error);
  },
);

export default api;

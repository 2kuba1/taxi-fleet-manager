"use server";

import { cookies } from "next/headers";
import TokenResponse from "../interfaces/TokenResponse";
import { jwtDecode } from "jwt-decode";

export interface DotNetJwtPayload {
  aud: string;
  iss: string;
  exp: number;
  iat: number;
  nbf: number;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
    | string
    | string[];
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname": string;
}

export interface UserProfile {
  id: string;
  firstName: string;
  lastName: string;
  isAdmin: boolean;
}

export async function setAuthCookies(tokens: TokenResponse) {
  const cookieStore = await cookies();

  cookieStore.set(
    "userToken",
    JSON.stringify({
      accessToken: tokens.accessToken,
      refreshToken: tokens.refreshToken,
    }),
    {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "strict",
      maxAge: 30 * 24 * 60 * 60,
      path: "/",
    },
  );

  return { success: true };
}

export async function clearAuthCookies() {
  const cookieStore = await cookies();
  cookieStore.delete("userToken");
}

export async function getUserProfile(): Promise<UserProfile | null> {
  try {
    const cookieStore = await cookies();
    const tokenString = cookieStore.get("userToken")?.value;

    if (!tokenString) return null;

    const { accessToken } = JSON.parse(tokenString);
    const raw = jwtDecode<DotNetJwtPayload>(accessToken);

    const roleClaim =
      raw["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const isAdmin = Array.isArray(roleClaim)
      ? roleClaim.includes("Admin")
      : roleClaim === "Admin";

    return {
      id: raw[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ],
      firstName:
        raw["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
      lastName:
        raw["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"],
      isAdmin: isAdmin,
    };
  } catch (error) {
    console.error("Błąd dekodowania tokenu:", error);
    return null;
  }
}

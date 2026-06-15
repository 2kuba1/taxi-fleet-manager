"use server";

import { cookies } from "next/headers";
import TokenResponse from "../interfaces/TokenResponse";

export async function setAuthCookies(tokens: TokenResponse) {
  const cookieStore = await cookies();

  cookieStore.set("userToken", JSON.stringify({
    accessToken: tokens.accessToken,
    refreshToken: tokens.refreshToken
  }), {
    httpOnly: true,  
    secure: process.env.NODE_ENV === "production",
    sameSite: "strict",
    maxAge: 30 * 24 * 60 * 60,
    path: "/",
  });

  return { success: true };
}

export async function clearAuthCookies() {
  const cookieStore = await cookies();
  cookieStore.delete("userToken");
}
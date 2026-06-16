"use client";

import Image from "next/image";
import { useState } from "react";
import axios from "axios";
import TokenResponse from "./interfaces/TokenResponse";
import { setAuthCookies } from "./actions/auth";
import { MoonLoader } from "react-spinners";

export default function Home() {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    if (!login) {
      setError("Login nie może być pusty.");
      setLoading(false);
      return;
    }

    if (!password) {
      setError("Hasło nie może być puste.");
      setLoading(false);
      return;
    }

    try {
      const response = await axios.post(
        process.env.NEXT_PUBLIC_API_URL + "/api/auth/login",
        {
          login: login,
          password: password,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        },
      );
      await setAuthCookies(response.data as TokenResponse);
      console.log(response);
    } catch (error) {
      console.log(error);
      setError(
        "Nie udało się zalogować. Sprawdź swoje dane i spróbuj ponownie.",
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col min-h-screen items-center justify-between font-sans p-6 bg-white">
      <main className="flex flex-col items-center w-full max-w-sm">
        <Image
          src="/okaycieszyn.png"
          alt="Okay taxi cieszyn logo"
          width={250}
          height={250}
          priority
        />

        <h1 className="text-3xl font-bold text-center">
          Okay Taxi Cieszyn
          <br />
          Fleet Manager
        </h1>

        <form
          className="flex flex-col w-full max-w-xs mt-8 space-y-3"
          onSubmit={handleSubmit}
        >
          <label
            htmlFor="login"
            className="text-lg font-semibold text-tertiary"
          >
            Login
          </label>
          <input
            name="login"
            type="text"
            placeholder="Wpisz swój login"
            className="border-black border-2 rounded-md px-4 py-3 w-full text-base focus:outline-none"
            value={login}
            onChange={(e) => setLogin(e.target.value)}
          />
          <label
            htmlFor="password"
            className="text-lg font-semibold text-tertiary"
          >
            Hasło
          </label>
          <input
            type="password"
            placeholder="Wpisz hasło"
            name="password"
            className="border-black border-2 rounded-md px-4 py-3 w-full text-base focus:outline-none"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <button
            type="submit"
            className="bg-[#d61e24] hover:bg-red-700 transition-colors text-white font-medium rounded-md px-4 py-3 w-full text-base mt-2"
          >
            {loading ? <MoonLoader size={20} color="white" /> : "Zaloguj się"}
          </button>
          {error && <p className="text-red-500 text-sm mt-2">{error}</p>}
        </form>
      </main>

      <div className="text-xs text-gray-400 mt-6">© 2026 Jakub Wojtyna</div>
    </div>
  );
}

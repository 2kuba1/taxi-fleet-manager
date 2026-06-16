"use client";

import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import Image from "next/image";
import axios from "axios";
import { MoonLoader } from "react-spinners";
import { useRouter } from "next/navigation";

export default function SetupPasswordPage() {
  const searchParams = useSearchParams();

  const token = searchParams.get("token");
  const email = searchParams.get("email");

  const router = useRouter();

  const [isLoading, setIsLoading] = useState(false);
  const [validationError, setValidationError] = useState<string | null>(null);
  const [resetMessage, setResetMessage] = useState<{
    type: "success" | "error";
    text: string;
  } | null>(null);

  const [temporaryPassword, setTemporaryPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");

  useEffect(() => {
    const check = async () => {
      if (!token) {
        return;
      }
      if (!email) {
        return;
      }

      try {
        await axios.post(
          process.env.NEXT_PUBLIC_API_URL + "/api/auth/validate-reset-token",
          {
            token: token,
            email: email,
          },
          {
            headers: {
              "Content-Type": "application/json",
            },
          },
        );
      } catch (error) {
        setValidationError(
          "Nieprawidłowy lub wygasły token. Proszę wygenerować nowy link resetujący hasło.",
        );
      }
    };
    check();
  }, [token, email]);

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (newPassword !== confirmNewPassword) {
      setResetMessage({
        type: "error",
        text: "Nowe hasło i jego potwierdzenie nie są takie same.",
      });
      return;
    }

    if (!token) {
      setResetMessage({
        type: "error",
        text: "Brak tokenu. Proszę użyć linku z maila.",
      });
      return;
    }

    if (!temporaryPassword) {
      setResetMessage({
        type: "error",
        text: "Proszę wprowadzić tymczasowe hasło z maila.",
      });
      return;
    }

    setIsLoading(true);

    console.log({
      token,
      email,
      temporaryPassword,
      newPassword,
    });

    try {
      await axios.post(
        process.env.NEXT_PUBLIC_API_URL + "/api/auth/setup-password",
        {
          token: token,
          email: email,
          temporaryPassword: temporaryPassword,
          newPassword: newPassword,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        },
      );

      router.push("/?reset-success=true");
    } catch (error: any) {
      setResetMessage({
        type: "error",
        text: "Wystąpił błąd podczas resetowania hasła.",
      });
    } finally {
      setIsLoading(false);
    }
  };

  if (!token || !email || validationError) {
    return (
      <div className="text-center p-10">
        <h1 className="text-2xl font-bold text-red-500">Błąd dostępu</h1>
        <p>Link aktywacyjny jest nieprawidłowy lub wygasł.</p>
      </div>
    );
  }

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

        <h1 className="text-3xl font-bold text-center">Ustaw nowe hasło</h1>

        <form
          className="flex flex-col w-full max-w-xs mt-8 space-y-3"
          onSubmit={handleSubmit}
        >
          <label
            htmlFor="temporrary-password"
            className="text-lg font-semibold text-tertiary"
          >
            Tymczasowe hasło z maila
          </label>
          <input
            name="temporrary-password"
            type="password"
            placeholder="Wpisz swoje tymczasowe hasło z maila"
            className="border-black border-2 rounded-md px-4 py-3 w-full text-base focus:outline-none"
            value={temporaryPassword}
            onChange={(e) => setTemporaryPassword(e.target.value)}
          />
          <label
            htmlFor="new-password"
            className="text-lg font-semibold text-tertiary"
          >
            Nowe hasło
          </label>
          <input
            type="password"
            placeholder="Wpisz nowe hasło"
            name="new-password"
            className="border-black border-2 rounded-md px-4 py-3 w-full text-base focus:outline-none"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
          />
          <label
            htmlFor="new-password-confirm"
            className="text-lg font-semibold text-tertiary"
          >
            Powtórz hasło
          </label>
          <input
            type="password"
            placeholder="Powtórz nowe hasło"
            name="new-password-confirm"
            className="border-black border-2 rounded-md px-4 py-3 w-full text-base focus:outline-none"
            value={confirmNewPassword}
            onChange={(e) => setConfirmNewPassword(e.target.value)}
          />
          <button
            type="submit"
            className="bg-[#d61e24] hover:bg-red-700 transition-colors text-white font-medium rounded-md px-4 py-3 w-full text-base mt-2"
          >
            {isLoading ? (
              <MoonLoader size={20} color="white" />
            ) : (
              "Ustaw nowe hasło"
            )}
          </button>
        </form>
        {resetMessage && (
          <p
            className={`text-sm mt-4 ${resetMessage.type === "success" ? "text-green-500" : "text-red-500"}`}
          >
            {resetMessage.text}
          </p>
        )}
      </main>
      <div className="text-xs text-gray-400 mt-6">
        © 2026 Taxi Fleet Manager
      </div>
    </div>
  );
}

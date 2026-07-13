"use client";

import { useEffect, useState } from "react";
import {
  User,
  LogOut,
  Calendar,
  Activity,
  ClipboardList,
  CheckCircle2,
  Clock,
} from "lucide-react";
import Navbar from "../../components/Navbar";
import { clearAuthCookies } from "@/app/actions/auth";
import { useRouter } from "next/navigation";

const MOCK_REPORTS = [
  {
    id: "1",
    date: "2026-07-12",
    car: "Octavia",
    kilometers: 184,
    cashless: "420.50 zł",
    status: "Rozliczony",
  },
  {
    id: "2",
    date: "2026-07-10",
    car: "Citroen",
    kilometers: 210,
    cashless: "610.00 zł",
    status: "W weryfikacji",
  },
  {
    id: "3",
    date: "2026-07-09",
    car: "Toyota",
    kilometers: 145,
    cashless: "315.20 zł",
    status: "Rozliczony",
  },
];

export default function ProfilePage() {
  const router = useRouter();

  const [user, setUser] = useState<{
    name?: string;
    surname?: string;
    email?: string;
  } | null>(null);

  const [loading, setLoading] = useState(true);

  const handleLogout = async () => {
    clearAuthCookies();
    router.push("/");
  };

  return (
    <div className="flex flex-col min-h-screen items-center justify-start font-sans p-4 bg-[#F8F9FA] text-black">
      <main className="flex flex-col w-full max-w-md mt-6 pb-24">
        <h1 className="text-[#B30006] text-2xl font-black uppercase tracking-tight">
          Profil Kierowcy
        </h1>
        <p className="text-gray-600 text-sm mt-1 font-medium leading-snug">
          Twoje statystyki, dane konta oraz historia przesłanych raportów.
        </p>

        <div className="flex flex-col w-full mt-4 bg-white border border-gray-400 rounded-sm p-4 shadow-sm">
          <div className="flex items-center gap-4">
            <div className="flex items-center justify-center bg-gray-100 border-2 border-black w-14 h-14 rounded-sm shrink-0">
              <User className="w-8 h-8 text-black" />
            </div>
            <div className="flex flex-col overflow-hidden">
              <h2 className="font-black text-lg uppercase tracking-tight truncate">
                {loading ? "Ładowanie..." : user?.name || "Kierowca Testowy"}
              </h2>
              <p className="text-xs text-gray-500 font-medium truncate">
                {loading ? "..." : user?.email || "kierowca@taxi.pl"}
              </p>
            </div>
          </div>

          <hr className="border-gray-200 my-4" />

          <div className="grid grid-cols-2 gap-3">
            <div className="border border-gray-300 rounded-sm p-3 bg-[#F8F9FA]">
              <div className="flex items-center gap-1.5 text-xs font-black uppercase tracking-wider text-gray-500 mb-1">
                <Activity className="w-4 h-4 text-[#B30006]" />
                <span>Dystans (msc)</span>
              </div>
              <p className="text-xl font-black">539 km</p>
            </div>
            <div className="border border-gray-300 rounded-sm p-3 bg-[#F8F9FA]">
              <div className="flex items-center gap-1.5 text-xs font-black uppercase tracking-wider text-gray-500 mb-1">
                <ClipboardList className="w-4 h-4 text-[#B30006]" />
                <span>Raporty (msc)</span>
              </div>
              <p className="text-xl font-black">12 szt.</p>
            </div>
          </div>

          <button
            onClick={handleLogout}
            className="flex items-center justify-center gap-2 w-full mt-4 border-2 border-black text-black font-black uppercase tracking-wide text-xs py-2.5 hover:bg-gray-100 transition active:scale-[0.99]"
          >
            <LogOut className="w-4 h-4" />
            Wyloguj się
          </button>
        </div>

        <div className="flex flex-col w-full mt-6">
          <h3 className="text-xs font-black uppercase tracking-wider mb-2">
            Historia ostatnich raportów
          </h3>

          <div className="space-y-3">
            {MOCK_REPORTS.map((report) => (
              <div
                key={report.id}
                className="flex flex-col w-full bg-white border border-gray-400 rounded-sm p-4 shadow-sm"
              >
                <div className="flex justify-between items-start">
                  <div className="flex items-center gap-2">
                    <Calendar className="w-4 h-4 text-gray-500" />
                    <span className="font-black text-sm text-black">
                      {report.date}
                    </span>
                  </div>

                  <div
                    className={`flex items-center gap-1 px-2 py-0.5 rounded-sm border text-[10px] font-black uppercase tracking-wider ${
                      report.status === "Rozliczony"
                        ? "bg-green-50 border-green-500 text-green-700"
                        : "bg-amber-50 border-amber-500 text-amber-700"
                    }`}
                  >
                    {report.status === "Rozliczony" ? (
                      <CheckCircle2 className="w-3 h-3" />
                    ) : (
                      <Clock className="w-3 h-3" />
                    )}
                    <span>{report.status}</span>
                  </div>
                </div>

                <div className="grid grid-cols-3 gap-2 mt-3 text-xs border-t border-gray-100 pt-3">
                  <div>
                    <p className="text-gray-400 font-bold uppercase tracking-tight text-[10px]">
                      Pojazd
                    </p>
                    <p className="font-black text-gray-800 truncate">
                      {report.car}
                    </p>
                  </div>
                  <div>
                    <p className="text-gray-400 font-bold uppercase tracking-tight text-[10px]">
                      Dystans
                    </p>
                    <p className="font-black text-gray-800">
                      {report.kilometers} km
                    </p>
                  </div>
                  <div className="text-right">
                    <p className="text-gray-400 font-bold uppercase tracking-tight text-[10px]">
                      Karta
                    </p>
                    <p className="font-black text-[#B30006]">
                      {report.cashless}
                    </p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </main>

      <Navbar />
    </div>
  );
}

"use client";

import { useState } from "react";
import CameraCapture from "../components/CameraCapture";
import { createShiftReport } from "../actions/shiftReport";

const CARS = [
  { id: "1", brand: "Toyota", model: "Corola" },
  { id: "2", brand: "Skoda", model: "Octavia" },
  { id: "3", brand: "Citroen", model: "C-elise" },
  { id: null, brand: "Brak", model: "Brak" },
];

export default function CreateReportPage() {
  const [photo, setPhoto] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (isSubmitting) return;

    setIsSubmitting(true);
    const formElements = new FormData(e.currentTarget);

    try {
      let imageBlob: Blob | null = null;

      if (photo) {
        imageBlob = await fetch(photo).then((res) => res.blob());
      }

      const data = new FormData();

      if (imageBlob) {
        data.append("image", imageBlob, "odometer.jpg");
      }

      const kilometers = formElements.get("kilometers");
      data.append("kilometersDriven", kilometers ? String(kilometers) : "0");

      const cashless = String(formElements.get("cashlessTransactions")).replace(
        ",",
        ".",
      );
      data.append("cardTransactionsSum", cashless);

      data.append("shiftDay", new Date().toISOString());

      const carIdValue = formElements.get("carId");
      if (carIdValue && carIdValue !== "") {
        data.append("carId", String(carIdValue));
      }

      await createShiftReport(data);
    } catch (error: any) {
      console.log(error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex flex-col min-h-screen items-center justify-start font-sans p-4 bg-[#F8F9FA] text-black">
      <main className="flex flex-col w-full max-w-100 mt-6">
        <h1 className="text-[#B30006] text-2xl font-black uppercase tracking-tight">
          Zakończenie zmiany
        </h1>
        <p className="text-gray-600 text-sm mt-1 font-medium leading-snug">
          Uzupełnij raport końcowy, aby rozliczyć dzisiejsze przejazdy.
        </p>

        <form
          onSubmit={handleSubmit}
          suppressHydrationWarning
          className="flex flex-col w-full mt-4 bg-white border border-gray-400 rounded-sm p-4 shadow-sm space-y-5"
        >
          <div className="flex flex-col w-full">
            <label className="text-xs font-black uppercase tracking-wider mb-2">
              Wybierz samochód
            </label>
            <div className="relative flex items-center">
              <span className="absolute left-3 text-gray-500 pointer-events-none">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-5 h-5"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M8.25 18.75a1.5 1.5 0 0 1-3 0m3 0a1.5 1.5 0 0 0-3 0m3 0h6m-9 0H3.375a1.125 1.125 0 0 1-1.125-1.125V14.25m17.25 4.5a1.5 1.5 0 0 1-3 0m3 0a1.5 1.5 0 0 0-3 0m3 0h1.125c.621 0 1.129-.504 1.129-1.125v-3.02M14.25 18.75v-3m-7.5-1.5h13.5m-16.5 0h3m-3 0a1.125 1.125 0 0 1-1.125-1.125V10.5a3.75 3.75 0 0 1 3.75-3.75h10.5a3.75 3.75 0 0 1 3.75 3.75v2.25c0 .621-.504 1.125-1.125 1.125h-1.5m-.75-3h-3.75m0 0v-3m-3.75 3H7.5m3 0v-3"
                  />
                </svg>
              </span>
              <select
                name="carId"
                required
                suppressHydrationWarning
                defaultValue=""
                className="w-full border-2 border-black rounded-sm py-2.5 pl-10 pr-10 text-sm font-medium bg-white appearance-none focus:outline-none cursor-pointer"
              >
                <option value="" disabled hidden>
                  Wybierz pojazd z listy
                </option>
                {CARS.map((car) => (
                  <option key={car.id ?? "no-id"} value={car.id ?? ""}>
                    {car.brand} {car.model}
                  </option>
                ))}
              </select>
              <span className="absolute right-3 text-gray-500 pointer-events-none">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={2}
                  stroke="currentColor"
                  className="w-4 h-4"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="m19.5 8.25-7.5 7.5-7.5-7.5"
                  />
                </svg>
              </span>
            </div>
          </div>

          <div className="flex flex-col w-full">
            <label className="text-xs font-black uppercase tracking-wider mb-2">
              Zrób zdjęcie licznika kilometrów
            </label>
            <CameraCapture onPhotoCaptured={setPhoto} />
            <input type="hidden" name="odometerPhoto" value={photo || ""} />
          </div>

          <div className="flex flex-col w-full">
            <label className="text-xs font-black uppercase tracking-wider mb-2">
              Kilometry na zmianie
            </label>
            <div className="relative flex items-center">
              <span className="absolute left-3 text-gray-500">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-5 h-5"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M15.3 16.2c-1.396 1.157-3.204 1.8-5.3 1.8a9.96 9.96 0 0 1-6.947-2.833M17.25 15.75l1.5 1.5M19.5 10.5a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
                  />
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M10.5 6.75a3.75 3.75 0 1 0 0 7.5 3.75 3.75 0 0 0 0-7.5Z"
                  />
                </svg>
              </span>
              <input
                type="number"
                name="kilometers"
                required
                min="0"
                suppressHydrationWarning
                placeholder="Wprowadź stan licznika"
                className="w-full border-2 border-black rounded-sm py-2.5 pl-10 pr-4 text-sm font-medium placeholder-gray-400 focus:outline-none"
              />
            </div>
          </div>

          <div className="flex flex-col w-full">
            <label className="text-xs font-black uppercase tracking-wider mb-2">
              Suma transakcji bezgotówkowych
            </label>
            <div className="relative flex items-center">
              <span className="absolute left-3 text-gray-500">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-5 h-5"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M2.25 8.25h19.5M2.25 9h19.5m-16.5 5.25h6m-6 2.25h3m-3.75 3h15a2.25 2.25 0 0 0 2.25-2.25V6.75A2.25 2.25 0 0 0 19.5 4.5h-15a2.25 2.25 0 0 0-2.25 2.25v10.5A2.25 2.25 0 0 0 4.5 19.5Z"
                  />
                </svg>
              </span>
              <input
                type="number"
                name="cashlessTransactions"
                required
                min="0"
                step="0.01"
                suppressHydrationWarning
                placeholder="0.00"
                className="w-full border-2 border-black rounded-sm py-2.5 pl-10 pr-4 text-sm font-medium placeholder-gray-400 focus:outline-none"
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full bg-[#D3131E] text-white font-black uppercase tracking-wide text-md py-3 shadow-md hover:bg-[#b80f18] transition active:scale-[0.99] disabled:bg-gray-400 disabled:cursor-not-allowed"
          >
            {isSubmitting ? "Wysyłanie..." : "Wyślij raport ze zmiany"}
          </button>
        </form>
      </main>
    </div>
  );
}

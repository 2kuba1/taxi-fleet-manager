"use client";

import { useState } from "react";
import {
  Car,
  Gauge,
  CreditCard,
  ChevronDown,
  Calendar,
  Calendar1,
  CalendarCheck,
} from "lucide-react";

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
        data.append("Image", imageBlob, "odometer.jpg");
      }

      const kilometers = formElements.get("kilometers");
      data.append("KilometersDriven", kilometers ? String(kilometers) : "0");

      const cashless = String(formElements.get("cashlessTransactions")).replace(
        ",",
        ".",
      );
      data.append("CardTransactionsSum", cashless);

      const date = formElements.get("date");
      data.append("ShiftDate", date ? String(date) : new Date().toISOString());

      const carIdValue = formElements.get("carId");
      if (carIdValue && carIdValue !== "") {
        data.append("CarId", String(carIdValue));
      }

      await createShiftReport(data);
    } catch (error: any) {
      console.error("Błąd po stronie klienta:", error);
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
                <Car className="w-5 h-5" />
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
                <ChevronDown className="w-4 h-4" />
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
                <Gauge className="w-5 h-5" />
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
                <CreditCard className="w-5 h-5" />
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

          <div className="flex flex-col w-full">
            <label className="text-xs font-black uppercase tracking-wider mb-2">
              Data
            </label>
            <div className="relative flex items-center">
              <span className="absolute left-3 text-gray-500">
                <CalendarCheck className="w-5 h-5" />
              </span>
              <input
                type="date"
                name="date"
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

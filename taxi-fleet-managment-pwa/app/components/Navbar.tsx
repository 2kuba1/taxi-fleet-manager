"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import { CalendarDays, User, Home, FilePlus } from "lucide-react";

export default function Navbar() {
  const pathname = usePathname();

  const navLinks = [
    { href: "/", label: "Home", icon: Home },
    { href: "/schedule", label: "Grafik", icon: CalendarDays },
    { href: "/create-report", label: "Nowy Raport", icon: FilePlus },
    { href: "/profile", label: "Profil", icon: User },
  ];

  return (
    <nav className="fixed bottom-0 left-0 right-0 border-t-2 py-2 px-4 shadow-lg bg-secondary/50 z-50">
      <div className="mx-auto flex max-w-md justify-between items-center px-4">
        {navLinks.map((link) => {
          const Icon = link.icon;
          const isActive = pathname === link.href;

          return (
            <Link
              key={link.href}
              href={link.href}
              className={`flex flex-col items-center gap-1 transition-all duration-200 ${
                isActive
                  ? "bg-[#b3001b] text-white px-5 py-2 rounded-xl shadow-md min-w-20"
                  : "text-muted-foreground hover:text-foreground px-3 py-2"
              }`}
            >
              <Icon className="h-6 w-6" />
              <span className="text-xs font-medium">{link.label}</span>
            </Link>
          );
        })}
      </div>
    </nav>
  );
}

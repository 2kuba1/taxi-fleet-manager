"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import { useEffect, useState } from "react";
import { CalendarDays, User, Home, FilePlus, ShieldUser } from "lucide-react";
import { getUserProfile } from "../actions/auth";

export default function Navbar() {
  const pathname = usePathname();
  const [isAdmin, setIsAdmin] = useState<boolean>(false);

  useEffect(() => {
    async function checkRole() {
      const profile = await getUserProfile();
      setIsAdmin(profile?.isAdmin ?? false);
    }
    checkRole();
  }, [pathname]);

  const navLinks = [
    { href: "/", label: "Home", icon: Home },
    { href: "/schedule", label: "Grafik", icon: CalendarDays },
    { href: "/create-report", label: "Nowy Raport", icon: FilePlus },
    { href: "/profile", label: "Profil", icon: User },
  ];

  return (
    <nav className="fixed bottom-0 left-0 right-0 border-t-2 py-2 px-2 shadow-lg bg-secondary/25 z-50 backdrop-blur-sm">
      <div className="mx-auto flex max-w-md justify-around items-center gap-1">
        {navLinks.map((link) => {
          const Icon = link.icon;
          const isActive = pathname === link.href;

          return (
            <Link
              key={link.href}
              href={link.href}
              className={`flex flex-1 flex-col items-center justify-center gap-1 py-2 px-1 rounded-xl transition-all duration-200 text-center ${
                isActive
                  ? "bg-[#b3001b] text-white shadow-md"
                  : "text-muted-foreground hover:text-foreground"
              }`}
            >
              <Icon className="h-5 w-5 sm:h-6 sm:w-6 shrink-0" />
              <span className="text-[10px] sm:text-xs font-medium whitespace-nowrap">
                {link.label}
              </span>
            </Link>
          );
        })}

        {isAdmin && (
          <Link
            href="/admin"
            className={`flex flex-1 flex-col items-center justify-center gap-1 py-2 px-1 rounded-xl transition-all duration-200 text-center ${
              pathname === "/admin"
                ? "bg-[#b3001b] text-white shadow-md"
                : "text-muted-foreground hover:text-foreground"
            }`}
          >
            <ShieldUser className="h-5 w-5 sm:h-6 sm:w-6 shrink-0" />
            <span className="text-[10px] sm:text-xs font-medium whitespace-nowrap">
              Admin
            </span>
          </Link>
        )}
      </div>
    </nav>
  );
}

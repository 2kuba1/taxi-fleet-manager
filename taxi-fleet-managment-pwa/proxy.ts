import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";
import { DotNetJwtPayload } from "./app/actions/auth";
import { jwtDecode } from "jwt-decode";

export function proxy(request: NextRequest) {
  const token = request.cookies.get("userToken")?.value;
  const { pathname } = request.nextUrl;

  if (!token) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  if (pathname.startsWith("/admin")) {
    try {
      const { accessToken } = JSON.parse(token);
      const decoded = jwtDecode<DotNetJwtPayload>(accessToken);

      const roleClaim =
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      const isAdmin = Array.isArray(roleClaim)
        ? roleClaim.includes("Admin")
        : roleClaim === "Admin";

      if (!isAdmin) {
        return NextResponse.redirect(new URL("/", request.url));
      }
    } catch (error) {
      return NextResponse.redirect(new URL("/", request.url));
    }

    return NextResponse.next();
  }
}

export const config = {
  matcher: ["/create-report/:path*", "/admin/:path*", "/profile/:path*"],
};

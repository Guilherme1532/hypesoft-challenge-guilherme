import { type ReactNode, useState } from "react";
import { Link } from "react-router-dom";
import {
  Boxes,
  FolderTree,
  LayoutDashboard,
  LogOut,
  Menu,
  Settings,
  X,
} from "lucide-react";
import { getUserProfile, logout } from "@/services/auth";
import { Button } from "@/components/ui/button";

type NavKey = "dashboard" | "products" | "categories";

interface AppShellProps {
  activeNav: NavKey;
  title: string;
  subtitle: string;
  children: ReactNode;
}

const navItems: Array<{ key: NavKey; label: string; href: string; icon: React.ComponentType<{ className?: string }> }> = [
  { key: "dashboard", label: "Dashboard", href: "/dashboard", icon: LayoutDashboard },
  { key: "products", label: "Produtos", href: "/products", icon: Boxes },
  { key: "categories", label: "Categorias", href: "/categories", icon: FolderTree },
];

export default function AppShell({ activeNav, title, subtitle, children }: AppShellProps) {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const user = getUserProfile();
  const initials = user.displayName
    .split(" ")
    .map((part) => part[0])
    .join("")
    .slice(0, 2)
    .toUpperCase();

  const roleLabel =
    user.role === "admin"
      ? "Admin"
      : user.role === "manager"
        ? "Manager"
        : user.role === "user"
          ? "User"
          : "Unknown";

  function closeMobileMenu() {
    setMobileMenuOpen(false);
  }

  function renderSidebarContent(mobile = false) {
    return (
      <>
        <div className="mb-5 flex items-center gap-3 sm:mb-8">
          <div className="flex size-10 items-center justify-center rounded-xl bg-indigo-600 text-white">
            <LayoutDashboard className="size-5" />
          </div>
          <div>
            <p className="text-xl font-semibold text-zinc-900">Storage</p>
            <p className="text-xs text-zinc-500">Controle de estoque</p>
          </div>
        </div>

        <div className="space-y-2 sm:space-y-3">
          <p className="px-3 text-xs font-medium tracking-[0.2em] text-zinc-400">GENERAL</p>
          <div
            className={
              mobile
                ? "space-y-2"
                : "flex gap-2 overflow-x-auto pb-1 lg:block lg:space-y-2 lg:overflow-visible lg:pb-0"
            }
          >
            {navItems.map((item) => (
              <Link
                key={item.key}
                to={item.href}
                onClick={mobile ? closeMobileMenu : undefined}
                className={`flex items-center gap-3 rounded-xl px-3 py-2.5 text-left text-sm transition ${
                  mobile ? "w-full" : "shrink-0 whitespace-nowrap lg:w-full"
                } ${
                  activeNav === item.key
                    ? "bg-zinc-900 text-white shadow-sm"
                    : "text-zinc-700 hover:bg-zinc-200/70"
                }`}
              >
                <item.icon className="size-4" />
                {item.label}
              </Link>
            ))}
          </div>
        </div>

        <div className={`mt-6 space-y-3 ${mobile ? "block" : "hidden lg:block"}`}>
          <p className="px-3 text-xs font-medium tracking-[0.2em] text-zinc-400">SUPPORT</p>
          <button
            type="button"
            className="flex w-full items-center gap-3 rounded-xl px-3 py-2.5 text-left text-sm text-zinc-700 transition hover:bg-zinc-200/70"
          >
            <Settings className="size-4" />
            Configuracoes
          </button>
          <button
            type="button"
            onClick={() => {
              if (mobile) closeMobileMenu();
              logout();
            }}
            className="flex w-full items-center gap-3 rounded-xl px-3 py-2.5 text-left text-sm text-zinc-700 transition hover:bg-zinc-200/70"
          >
            <LogOut className="size-4" />
            Sair
          </button>
        </div>
      </>
    );
  }

  return (
    <div className="min-h-screen bg-[#d7dbea] p-2 sm:p-4 md:p-6 lg:p-8">
      <main className="mx-auto w-full max-w-375 overflow-hidden rounded-2xl border border-white/70 bg-white shadow-2xl sm:rounded-3xl">
        <div className="grid min-h-215 lg:grid-cols-[280px_1fr]">
          <aside className="hidden border-r border-zinc-100 bg-zinc-50 p-4 sm:p-5 lg:block">
            {renderSidebarContent()}
          </aside>

          <div className="bg-zinc-50/60">
            <header className="border-b border-zinc-100 bg-white px-5 py-4 lg:px-7">
              <div className="flex items-center justify-between gap-3 lg:justify-end">
                <Button
                  variant="outline"
                  size="icon-sm"
                  className="rounded-xl lg:hidden"
                  onClick={() => setMobileMenuOpen(true)}
                >
                  <Menu />
                </Button>
                <div className="flex items-center justify-between gap-2 rounded-2xl border border-zinc-200 bg-zinc-50 px-2.5 py-2 sm:w-auto sm:justify-start sm:gap-3 sm:px-3">
                  <div className="flex size-9 items-center justify-center rounded-xl bg-linear-to-br from-indigo-600 to-violet-500 text-xs font-semibold text-white sm:size-10 sm:text-sm">
                    {initials}
                  </div>
                  <div className="min-w-0">
                    <p className="max-w-30 truncate text-xs font-semibold text-zinc-900 sm:max-w-none sm:text-sm">
                      {user.displayName}
                    </p>
                    <p className="text-[11px] text-zinc-500 sm:text-xs">Storage {roleLabel}</p>
                  </div>
                </div>
              </div>
            </header>

            <section className="space-y-4 p-3 sm:space-y-5 sm:p-5 lg:p-7">
              <div className="flex items-end justify-between">
                <div>
                  <h1 className="text-2xl font-semibold text-zinc-900 sm:text-3xl">{title}</h1>
                  <p className="text-sm text-zinc-500">{subtitle}</p>
                </div>
              </div>
              {children}
            </section>
          </div>
        </div>
      </main>

      {mobileMenuOpen && (
        <div className="fixed inset-0 z-50 lg:hidden">
          <button
            type="button"
            className="absolute inset-0 bg-black/40"
            onClick={closeMobileMenu}
            aria-label="Fechar menu"
          />
          <aside className="relative z-10 h-full w-[86%] max-w-[320px] border-r border-zinc-200 bg-zinc-50 p-4 shadow-xl">
            <div className="mb-4 flex items-center justify-between">
              <p className="text-sm font-medium text-zinc-600">Menu</p>
              <Button variant="outline" size="icon-sm" className="rounded-xl" onClick={closeMobileMenu}>
                <X />
              </Button>
            </div>
            {renderSidebarContent(true)}
          </aside>
        </div>
      )}
    </div>
  );
}

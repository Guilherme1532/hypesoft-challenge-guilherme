import { useState } from "react";
import { LogIn, ShieldCheck } from "lucide-react";
import { keycloak } from "@/lib/keycloak";
import { login } from "@/services/auth";
import { Navigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";

export default function LoginPage() {
  const [isSubmitting, setIsSubmitting] = useState(false);

  if (keycloak.authenticated) return <Navigate to="/dashboard" replace />;

  async function handleLogin() {
    try {
      setIsSubmitting(true);
      await login();
    } catch {
      setIsSubmitting(false);
    }
  }

  return (
    <main className="relative min-h-screen overflow-hidden bg-[#d7dbea] p-4 sm:p-8">
      <div className="pointer-events-none absolute inset-0">
        <div className="absolute -top-24 -left-20 h-72 w-72 rounded-full bg-indigo-300/50 blur-3xl" />
        <div className="absolute right-0 bottom-0 h-80 w-80 rounded-full bg-violet-300/40 blur-3xl" />
      </div>

      <div className="relative mx-auto flex min-h-[calc(100vh-2rem)] max-w-5xl items-center justify-center">
        <Card className="w-full max-w-md border-white/70 bg-white/95 shadow-2xl backdrop-blur">
          <CardHeader className="space-y-3 text-center">
            <div className="mx-auto flex size-12 items-center justify-center rounded-2xl bg-linear-to-br from-indigo-600 to-violet-500 text-white">
              <ShieldCheck className="size-6" />
            </div>
            <CardTitle className="text-2xl text-zinc-900">Entrar no Storage</CardTitle>
            <CardDescription>
              Autentique-se com sua conta corporativa no Keycloak.
            </CardDescription>
          </CardHeader>

          <CardContent className="space-y-4">
            <Button className="h-10 w-full" onClick={() => void handleLogin()} disabled={isSubmitting}>
              <LogIn />
              {isSubmitting ? "Redirecionando..." : "Entrar com Keycloak"}
            </Button>

            <p className="text-center text-xs text-zinc-500">
              O acesso e controlado por roles (admin, manager, user).
            </p>
          </CardContent>
        </Card>
      </div>
    </main>
  );
}

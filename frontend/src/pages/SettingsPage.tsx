import { LogOut, Moon, Sun, UserCog } from "lucide-react";
import AppShell from "@/components/layout/AppShell";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { getUserAccountDetails, logout, openAccountManagement } from "@/services/auth";
import { usePreferencesStore } from "@/stores/preferences-store";

function roleLabel(role: "admin" | "manager" | "user" | "unknown") {
  if (role === "admin") return "Admin";
  if (role === "manager") return "Manager";
  if (role === "user") return "User";
  return "Unknown";
}

export default function SettingsPage() {
  const account = getUserAccountDetails();
  const theme = usePreferencesStore((state) => state.theme);
  const density = usePreferencesStore((state) => state.density);
  const setTheme = usePreferencesStore((state) => state.setTheme);
  const setDensity = usePreferencesStore((state) => state.setDensity);

  return (
    <AppShell
      activeNav="settings"
      title="Configuracoes"
      subtitle="Preferencias de conta e interface."
    >
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <UserCog className="size-5" />
            Conta
          </CardTitle>
          <CardDescription>Dados do usuario autenticado no Keycloak.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid gap-3 sm:grid-cols-2">
            <div className="rounded-lg border bg-background p-3">
              <p className="text-xs text-muted-foreground">Nome</p>
              <p className="text-sm font-medium">{account.displayName}</p>
            </div>
            <div className="rounded-lg border bg-background p-3">
              <p className="text-xs text-muted-foreground">Username</p>
              <p className="text-sm font-medium">{account.username}</p>
            </div>
            <div className="rounded-lg border bg-background p-3">
              <p className="text-xs text-muted-foreground">Email</p>
              <p className="text-sm font-medium">{account.email}</p>
            </div>
            <div className="rounded-lg border bg-background p-3">
              <p className="text-xs text-muted-foreground">Role principal</p>
              <p className="text-sm font-medium">{roleLabel(account.role)}</p>
            </div>
          </div>

          <div className="flex flex-wrap gap-2">
            {account.roles.map((role) => (
              <Badge key={role} variant="secondary">
                {role}
              </Badge>
            ))}
          </div>

          <div className="flex flex-wrap gap-2">
            <Button variant="outline" onClick={() => openAccountManagement()}>
              Gerenciar conta no Keycloak
            </Button>
            <Button variant="destructive" onClick={() => logout()}>
              <LogOut />
              Sair
            </Button>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Aparencia</CardTitle>
          <CardDescription>Tema e densidade para a experiencia da interface.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-5">
          <div>
            <p className="mb-2 text-sm font-medium">Tema</p>
            <div className="flex flex-wrap gap-2">
              <Button
                variant={theme === "light" ? "default" : "outline"}
                onClick={() => setTheme("light")}
              >
                <Sun />
                Claro
              </Button>
              <Button
                variant={theme === "dark" ? "default" : "outline"}
                onClick={() => setTheme("dark")}
              >
                <Moon />
                Escuro
              </Button>
              <Button
                variant={theme === "system" ? "default" : "outline"}
                onClick={() => setTheme("system")}
              >
                Sistema
              </Button>
            </div>
          </div>

          <div>
            <p className="mb-2 text-sm font-medium">Densidade</p>
            <div className="flex flex-wrap gap-2">
              <Button
                variant={density === "comfortable" ? "default" : "outline"}
                onClick={() => setDensity("comfortable")}
              >
                Confortavel
              </Button>
              <Button
                variant={density === "compact" ? "default" : "outline"}
                onClick={() => setDensity("compact")}
              >
                Compacta
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>
    </AppShell>
  );
}

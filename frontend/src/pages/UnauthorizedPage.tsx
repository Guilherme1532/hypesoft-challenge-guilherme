import { Link } from "react-router-dom";
import { ShieldAlert } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function UnauthorizedPage() {
  return (
    <main className="flex min-h-screen items-center justify-center bg-zinc-100 p-4">
      <div className="w-full max-w-md rounded-2xl border bg-white p-8 text-center shadow-sm">
        <div className="mx-auto mb-4 flex size-12 items-center justify-center rounded-full bg-rose-100">
          <ShieldAlert className="size-6 text-rose-600" />
        </div>
        <h1 className="text-2xl font-semibold text-zinc-900">Acesso negado</h1>
        <p className="mt-2 text-sm text-zinc-600">
          Seu usuario nao possui permissao para acessar esta pagina.
        </p>
        <Button asChild className="mt-6">
          <Link to="/dashboard">Voltar para dashboard</Link>
        </Button>
      </div>
    </main>
  );
}

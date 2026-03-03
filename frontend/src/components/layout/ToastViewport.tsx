import { CheckCircle2, Info, X, XCircle } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useToastStore, type ToastType } from "@/stores/toast-store";

const stylesByType: Record<ToastType, string> = {
  success: "border-emerald-200 bg-emerald-50 text-emerald-900",
  error: "border-rose-200 bg-rose-50 text-rose-900",
  info: "border-zinc-200 bg-white text-zinc-900",
};

function Icon({ type }: { type: ToastType }) {
  if (type === "success") return <CheckCircle2 className="size-4" />;
  if (type === "error") return <XCircle className="size-4" />;
  return <Info className="size-4" />;
}

export default function ToastViewport() {
  const toasts = useToastStore((state) => state.toasts);
  const remove = useToastStore((state) => state.remove);

  if (!toasts.length) return null;

  return (
    <div className="pointer-events-none fixed right-4 bottom-4 z-100 flex w-full max-w-sm flex-col gap-2">
      {toasts.map((toast) => (
        <div
          key={toast.id}
          className={`pointer-events-auto rounded-xl border p-3 shadow-sm ${stylesByType[toast.type]}`}
        >
          <div className="flex items-start justify-between gap-3">
            <div className="flex items-start gap-2">
              <Icon type={toast.type} />
              <div>
                <p className="text-sm font-medium">{toast.title}</p>
                {toast.description && (
                  <p className="mt-0.5 text-xs opacity-90">{toast.description}</p>
                )}
              </div>
            </div>
            <Button
              size="icon-xs"
              variant="ghost"
              className="h-6 w-6"
              onClick={() => remove(toast.id)}
            >
              <X />
            </Button>
          </div>
        </div>
      ))}
    </div>
  );
}

import { useToastStore } from "@/stores/toast-store";

export function useToast() {
  const push = useToastStore((state) => state.push);

  return {
    success: (title: string, description?: string) =>
      push({ type: "success", title, description }),
    error: (title: string, description?: string) =>
      push({ type: "error", title, description }),
    info: (title: string, description?: string) =>
      push({ type: "info", title, description }),
  };
}

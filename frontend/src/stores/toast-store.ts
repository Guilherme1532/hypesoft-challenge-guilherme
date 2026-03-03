import { create } from "zustand";

export type ToastType = "success" | "error" | "info";

export interface ToastMessage {
  id: string;
  title: string;
  description?: string;
  type: ToastType;
}

interface ToastStore {
  toasts: ToastMessage[];
  push: (toast: Omit<ToastMessage, "id">) => void;
  remove: (id: string) => void;
}

const TOAST_TIMEOUT_MS = 3500;

export const useToastStore = create<ToastStore>((set) => ({
  toasts: [],
  push: (toast) => {
    const id = crypto.randomUUID();
    set((state) => ({ toasts: [...state.toasts, { ...toast, id }] }));

    setTimeout(() => {
      set((state) => ({ toasts: state.toasts.filter((message) => message.id !== id) }));
    }, TOAST_TIMEOUT_MS);
  },
  remove: (id) => {
    set((state) => ({ toasts: state.toasts.filter((message) => message.id !== id) }));
  },
}));

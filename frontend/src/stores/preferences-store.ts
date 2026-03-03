import { create } from "zustand";
import { persist } from "zustand/middleware";

export type ThemePreference = "light" | "dark" | "system";
export type DensityPreference = "comfortable" | "compact";

interface PreferencesState {
  theme: ThemePreference;
  density: DensityPreference;
  setTheme: (theme: ThemePreference) => void;
  setDensity: (density: DensityPreference) => void;
}

export const usePreferencesStore = create<PreferencesState>()(
  persist(
    (set) => ({
      theme: "system",
      density: "comfortable",
      setTheme: (theme) => set({ theme }),
      setDensity: (density) => set({ density }),
    }),
    {
      name: "hypesoft-preferences",
    }
  )
);

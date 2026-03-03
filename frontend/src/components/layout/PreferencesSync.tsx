import { useEffect } from "react";
import { usePreferencesStore } from "@/stores/preferences-store";

function getSystemPrefersDark() {
  return window.matchMedia("(prefers-color-scheme: dark)").matches;
}

export default function PreferencesSync() {
  const theme = usePreferencesStore((state) => state.theme);

  useEffect(() => {
    const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

    const applyTheme = () => {
      const shouldUseDark = theme === "dark" || (theme === "system" && getSystemPrefersDark());
      document.documentElement.classList.toggle("dark", shouldUseDark);
    };

    applyTheme();

    if (theme !== "system") {
      return;
    }

    const onMediaChange = () => applyTheme();
    mediaQuery.addEventListener("change", onMediaChange);
    return () => mediaQuery.removeEventListener("change", onMediaChange);
  }, [theme]);

  return null;
}

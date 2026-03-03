import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import App from "./App";
import "./index.css";
import { initAuth } from "@/services/auth";
import PreferencesSync from "@/components/layout/PreferencesSync";
import ToastViewport from "@/components/layout/ToastViewport";

const queryClient = new QueryClient();
const rootElement = document.getElementById("root");

if (!rootElement) {
  throw new Error("Root element #root was not found.");
}
const rootContainer = rootElement;

function renderApp() {
  ReactDOM.createRoot(rootContainer).render(
    <React.StrictMode>
      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
        <PreferencesSync />
        <ToastViewport />
      </QueryClientProvider>
    </React.StrictMode>
  );
}

async function bootstrap() {
  try {
    await initAuth();
  } catch (error) {
    console.error("Keycloak initialization failed. Rendering app without SSO session.", error);
  } finally {
    renderApp();
  }
}

bootstrap();

import { keycloak } from "@/lib/keycloak";

let initPromise: Promise<boolean> | null = null;
const APP_ORIGIN = window.location.origin;
const DASHBOARD_REDIRECT_URI = `${APP_ORIGIN}/dashboard`;
const LOGIN_REDIRECT_URI = `${APP_ORIGIN}/login`;

export function initAuth() {
  if (!initPromise) {
    initPromise = keycloak.init({
      onLoad: "check-sso", // não força login automático
      pkceMethod: "S256",
      checkLoginIframe: false, // evita loop/ruído em dev
    });
  }
  return initPromise;
}

export function login() {
  return keycloak.login({ redirectUri: DASHBOARD_REDIRECT_URI });
}

export function logout() {
  return keycloak.logout({ redirectUri: LOGIN_REDIRECT_URI });
}

export function getToken() {
  return keycloak.token;
}

export async function updateToken() {
  if (!keycloak.authenticated) return;
  await keycloak.updateToken(30);
}

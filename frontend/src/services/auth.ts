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

export function getUserRoles(): string[] {
  const parsed = keycloak.tokenParsed as
    | {
        roles?: string[] | string;
        realm_access?: { roles?: string[] };
      }
    | undefined;

  const directRoles =
    typeof parsed?.roles === "string"
      ? [parsed.roles]
      : Array.isArray(parsed?.roles)
        ? parsed.roles
        : [];

  const realmRoles = Array.isArray(parsed?.realm_access?.roles)
    ? parsed.realm_access.roles
    : [];

  return Array.from(new Set([...directRoles, ...realmRoles]));
}

export function hasAnyRole(roles: string[]) {
  const userRoles = getUserRoles();
  return roles.some((role) => userRoles.includes(role));
}

export interface UserProfile {
  displayName: string;
  role: "admin" | "manager" | "user" | "unknown";
}

function normalizeRole(roles: string[]): UserProfile["role"] {
  if (roles.includes("admin")) return "admin";
  if (roles.includes("manager")) return "manager";
  if (roles.includes("user")) return "user";
  return "unknown";
}

export function getUserProfile(): UserProfile {
  const parsed = keycloak.tokenParsed as
    | {
        preferred_username?: string;
        name?: string;
        given_name?: string;
        family_name?: string;
      }
    | undefined;

  const composedName =
    [parsed?.given_name, parsed?.family_name].filter(Boolean).join(" ").trim() || undefined;

  const displayName =
    parsed?.name?.trim() ||
    composedName ||
    parsed?.preferred_username?.trim() ||
    "Usuario";

  return {
    displayName,
    role: normalizeRole(getUserRoles()),
  };
}

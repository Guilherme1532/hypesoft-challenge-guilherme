const DEFAULT_API_BASE_URL = "/";
const DEFAULT_KEYCLOAK_URL = "http://localhost:8080";
const DEFAULT_KEYCLOAK_REALM = "hypesoft";
const DEFAULT_KEYCLOAK_CLIENT_ID = "frontend";

export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL?.trim() || DEFAULT_API_BASE_URL;

export const KEYCLOAK_URL =
  import.meta.env.VITE_KEYCLOAK_URL?.trim() || DEFAULT_KEYCLOAK_URL;

export const KEYCLOAK_REALM =
  import.meta.env.VITE_KEYCLOAK_REALM?.trim() || DEFAULT_KEYCLOAK_REALM;

export const KEYCLOAK_CLIENT_ID =
  import.meta.env.VITE_KEYCLOAK_CLIENT_ID?.trim() || DEFAULT_KEYCLOAK_CLIENT_ID;

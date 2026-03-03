import { Navigate } from "react-router-dom";
import { keycloak } from "@/lib/keycloak";

export default function RequireAuth({ children }: { children: React.ReactNode }) {
  if (!keycloak.authenticated) return <Navigate to="/login" replace />;
  return <>{children}</>;
}
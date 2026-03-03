import { keycloak } from "@/lib/keycloak";
import { login } from "@/services/auth";
import { Navigate } from "react-router-dom";

export default function LoginPage() {
  if (keycloak.authenticated) return <Navigate to="/dashboard" replace />;

  return (
    <div style={{ padding: 24 }}>
      <h1>Login</h1>
      <button onClick={() => login()}>Entrar com Keycloak</button>
    </div>
  );
}
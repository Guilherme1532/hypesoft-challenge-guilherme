import { Navigate } from "react-router-dom";
import { hasAnyRole } from "@/services/auth";

interface RequireRoleProps {
  children: React.ReactNode;
  roles: string[];
}

export default function RequireRole({ children, roles }: RequireRoleProps) {
  if (!hasAnyRole(roles)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
}

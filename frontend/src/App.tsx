import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "@/pages/LoginPage";
import DashboardPage from "@/pages/DashboardPage";
import ProductsPage from "@/pages/ProductsPage";
import ProductFormPage from "@/pages/ProductFormPage";
import CategoriesPage from "@/pages/CategoriesPage";
import UnauthorizedPage from "@/pages/UnauthorizedPage";
import SettingsPage from "@/pages/SettingsPage";
import RequireAuth from "@/components/layout/RequireAuth";
import RequireRole from "@/components/layout/RequireRole";

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/unauthorized" element={<UnauthorizedPage />} />
      <Route path="/" element={<Navigate to="/dashboard" replace />} />
      <Route
        path="/dashboard"
        element={
          <RequireAuth>
            <RequireRole roles={["user", "manager", "admin"]}>
              <DashboardPage />
            </RequireRole>
          </RequireAuth>
        }
      />
      <Route
        path="/products"
        element={
          <RequireAuth>
            <RequireRole roles={["user", "manager", "admin"]}>
              <ProductsPage />
            </RequireRole>
          </RequireAuth>
        }
      />
      <Route
        path="/products/new"
        element={
          <RequireAuth>
            <RequireRole roles={["manager", "admin"]}>
              <ProductFormPage />
            </RequireRole>
          </RequireAuth>
        }
      />
      <Route
        path="/products/:id/edit"
        element={
          <RequireAuth>
            <RequireRole roles={["manager", "admin"]}>
              <ProductFormPage />
            </RequireRole>
          </RequireAuth>
        }
      />
      <Route
        path="/categories"
        element={
          <RequireAuth>
            <RequireRole roles={["user", "manager", "admin"]}>
              <CategoriesPage />
            </RequireRole>
          </RequireAuth>
        }
      />
      <Route
        path="/settings"
        element={
          <RequireAuth>
            <RequireRole roles={["user", "manager", "admin"]}>
              <SettingsPage />
            </RequireRole>
          </RequireAuth>
        }
      />
    </Routes>
  );
}

import { logout } from "@/services/auth";

export default function DashboardPage() {
  return (
    <div style={{ padding: 24 }}>
      <h1>Dashboard</h1>
      <button onClick={() => logout()}>Sair</button>
    </div>
  );
}
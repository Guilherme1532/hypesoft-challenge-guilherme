import { apiClient } from "@/lib/api-client";
import type { DashboardSummary } from "@/types/api";

export async function getDashboardSummary() {
  const { data } = await apiClient.get<DashboardSummary>("/api/dashboard");
  return data;
}

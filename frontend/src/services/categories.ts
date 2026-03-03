import { apiClient } from "@/lib/api-client";
import type {
  Category,
  CreateCategoryPayload,
  UpdateCategoryPayload,
} from "@/types/api";

export async function listCategories() {
  const { data } = await apiClient.get<Category[]>("/api/categories");
  return data;
}

export async function getCategoryById(id: string) {
  const { data } = await apiClient.get<Category>(`/api/categories/${id}`);
  return data;
}

export async function createCategory(payload: CreateCategoryPayload) {
  const { data } = await apiClient.post<Category>("/api/categories", payload);
  return data;
}

export async function updateCategory(
  id: string,
  payload: UpdateCategoryPayload
) {
  await apiClient.put(`/api/categories/${id}`, { id, ...payload });
}

export async function deleteCategory(id: string) {
  await apiClient.delete(`/api/categories/${id}`);
}

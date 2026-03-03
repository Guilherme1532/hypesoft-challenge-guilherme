import { apiClient } from "@/lib/api-client";
import type {
  CreateProductPayload,
  PagedResult,
  Product,
  UpdateProductPayload,
  UpdateProductStockPayload,
} from "@/types/api";

export interface ListProductsParams {
  page?: number;
  pageSize?: number;
  categoryId?: string;
  search?: string;
}

export interface ListLowStockProductsParams {
  threshold?: number;
  limit?: number;
}

export async function listProducts(params: ListProductsParams = {}) {
  const { data } = await apiClient.get<PagedResult<Product>>("/api/products", {
    params,
  });
  return data;
}

export async function getProductById(id: string) {
  const { data } = await apiClient.get<Product>(`/api/products/${id}`);
  return data;
}

export async function createProduct(payload: CreateProductPayload) {
  const { data } = await apiClient.post<Product>("/api/products", payload);
  return data;
}

export async function updateProduct(id: string, payload: UpdateProductPayload) {
  await apiClient.put(`/api/products/${id}`, { id, ...payload });
}

export async function deleteProduct(id: string) {
  await apiClient.delete(`/api/products/${id}`);
}

export async function updateProductStock(
  id: string,
  payload: UpdateProductStockPayload
) {
  await apiClient.patch(`/api/products/${id}/stock`, payload);
}

export async function listLowStockProducts(
  params: ListLowStockProductsParams = {}
) {
  const { data } = await apiClient.get<Product[]>("/api/products/low-stock", {
    params,
  });
  return data;
}

import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { listCategories } from "@/services/categories";
import { getDashboardSummary } from "@/services/dashboard";
import {
  getProductById,
  listLowStockProducts,
  listProducts,
  type ListLowStockProductsParams,
  type ListProductsParams,
} from "@/services/products";

export const queryKeys = {
  dashboard: ["dashboard"] as const,
  categories: ["categories"] as const,
  products: (params: ListProductsParams) => ["products", params] as const,
  product: (id: string) => ["product", id] as const,
  lowStockProducts: (params: ListLowStockProductsParams) =>
    ["low-stock-products", params] as const,
};

export function useDashboardSummary() {
  return useQuery({
    queryKey: queryKeys.dashboard,
    queryFn: getDashboardSummary,
  });
}

export function useCategories() {
  return useQuery({
    queryKey: queryKeys.categories,
    queryFn: listCategories,
  });
}

export function useProducts(params: ListProductsParams = {}) {
  return useQuery({
    queryKey: queryKeys.products(params),
    queryFn: () => listProducts(params),
    placeholderData: keepPreviousData,
  });
}

export function useProductById(id: string, enabled = true) {
  return useQuery({
    queryKey: queryKeys.product(id),
    queryFn: () => getProductById(id),
    enabled,
  });
}

export function useLowStockProducts(params: ListLowStockProductsParams = {}) {
  return useQuery({
    queryKey: queryKeys.lowStockProducts(params),
    queryFn: () => listLowStockProducts(params),
  });
}

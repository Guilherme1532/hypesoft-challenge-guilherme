export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  categoryId: string;
  stockQuantity: number;
}

export interface Category {
  id: string;
  name: string;
}

export interface LowStockProduct {
  id: string;
  name: string;
  stockQuantity: number;
}

export interface ProductsByCategory {
  categoryId: string;
  count: number;
}

export interface DashboardSummary {
  totalProducts: number;
  totalStockValue: number;
  lowStockProducts: LowStockProduct[];
  productsByCategory: ProductsByCategory[];
}

export interface CreateProductPayload {
  name: string;
  description: string;
  price: number;
  categoryId: string;
  stockQuantity: number;
}

export type UpdateProductPayload = CreateProductPayload;

export interface UpdateProductStockPayload {
  stockQuantity: number;
}

export interface CreateCategoryPayload {
  name: string;
}

export interface UpdateCategoryPayload {
  name: string;
}

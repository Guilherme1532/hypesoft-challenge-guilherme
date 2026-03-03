import { useEffect } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Save } from "lucide-react";
import type { Category, Product } from "@/types/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { productSchema, type ProductFormValues } from "@/components/forms/schemas";

const defaultValues: ProductFormValues = {
  name: "",
  description: "",
  price: 0,
  categoryId: "",
  stockQuantity: 0,
};

interface ProductFormProps {
  product?: Product | null;
  categories: Category[];
  isCategoriesLoading: boolean;
  isSubmitting: boolean;
  showSubmitError: boolean;
  onSubmit: (values: ProductFormValues) => Promise<void>;
}

export default function ProductForm({
  product,
  categories,
  isCategoriesLoading,
  isSubmitting,
  showSubmitError,
  onSubmit,
}: ProductFormProps) {
  const form = useForm<ProductFormValues>({
    resolver: zodResolver(productSchema),
    defaultValues,
  });

  useEffect(() => {
    if (product) {
      form.reset({
        name: product.name ?? "",
        description: product.description ?? "",
        price: product.price,
        categoryId: product.categoryId ?? "",
        stockQuantity: product.stockQuantity,
      });
      return;
    }

    form.reset(defaultValues);
  }, [product, form]);

  return (
    <form className="space-y-4" onSubmit={form.handleSubmit((values) => void onSubmit(values))}>
      <div className="space-y-1.5">
        <label className="text-sm font-medium" htmlFor="name">
          Nome
        </label>
        <Input id="name" {...form.register("name")} />
        {form.formState.errors.name && (
          <p className="text-sm text-destructive">{form.formState.errors.name.message}</p>
        )}
      </div>

      <div className="space-y-1.5">
        <label className="text-sm font-medium" htmlFor="description">
          Descricao
        </label>
        <Input id="description" {...form.register("description")} />
        {form.formState.errors.description && (
          <p className="text-sm text-destructive">{form.formState.errors.description.message}</p>
        )}
      </div>

      <div className="grid gap-4 md:grid-cols-2">
        <div className="space-y-1.5">
          <label className="text-sm font-medium" htmlFor="price">
            Preco
          </label>
          <Input
            id="price"
            type="number"
            step="0.01"
            {...form.register("price", { valueAsNumber: true })}
          />
          {form.formState.errors.price && (
            <p className="text-sm text-destructive">{form.formState.errors.price.message}</p>
          )}
        </div>

        <div className="space-y-1.5">
          <label className="text-sm font-medium" htmlFor="stockQuantity">
            Estoque
          </label>
          <Input
            id="stockQuantity"
            type="number"
            {...form.register("stockQuantity", { valueAsNumber: true })}
          />
          {form.formState.errors.stockQuantity && (
            <p className="text-sm text-destructive">{form.formState.errors.stockQuantity.message}</p>
          )}
        </div>
      </div>

      <div className="space-y-1.5">
        <label className="text-sm font-medium" htmlFor="categoryId">
          Categoria
        </label>
        <select
          id="categoryId"
          className="h-9 w-full rounded-md border border-input bg-background px-3 text-sm shadow-xs outline-none"
          {...form.register("categoryId")}
          disabled={isCategoriesLoading}
        >
          <option value="">Selecione uma categoria</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {form.formState.errors.categoryId && (
          <p className="text-sm text-destructive">{form.formState.errors.categoryId.message}</p>
        )}
      </div>

      {showSubmitError && (
        <p className="text-sm text-destructive">
          Falha ao salvar produto. Verifique os dados e tente novamente.
        </p>
      )}

      <Button type="submit" disabled={isSubmitting} className="w-full sm:w-auto">
        <Save />
        {isSubmitting ? "Salvando..." : "Salvar produto"}
      </Button>
    </form>
  );
}

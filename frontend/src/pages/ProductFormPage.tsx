import { Link, useNavigate, useParams } from "react-router-dom";
import { ArrowLeft } from "lucide-react";
import ProductForm from "@/components/forms/ProductForm";
import type { ProductFormValues } from "@/components/forms/schemas";
import { useCreateProductMutation, useUpdateProductMutation } from "@/hooks/mutations";
import { useToast } from "@/hooks/use-toast";
import { useCategories, useProductById } from "@/hooks/queries";
import AppShell from "@/components/layout/AppShell";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";

export default function ProductFormPage() {
  const navigate = useNavigate();
  const toast = useToast();
  const { id } = useParams();
  const isEdit = Boolean(id);

  const { data: categories, isLoading: isCategoriesLoading } = useCategories();
  const { data: product, isLoading: isProductLoading } = useProductById(id ?? "", isEdit);

  const createMutation = useCreateProductMutation();
  const updateMutation = useUpdateProductMutation();

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  async function onSubmit(values: ProductFormValues) {
    try {
      if (isEdit && id) {
        await updateMutation.mutateAsync({
          id,
          payload: values,
        });
        toast.success("Produto atualizado", "Alteracoes salvas com sucesso.");
      } else {
        await createMutation.mutateAsync(values);
        toast.success("Produto criado", "Novo produto cadastrado com sucesso.");
      }

      navigate("/products");
    } catch {
      toast.error("Falha ao salvar produto", "Verifique os dados e tente novamente.");
    }
  }

  return (
    <AppShell
      activeNav="products"
      title={isEdit ? "Editar produto" : "Novo produto"}
      subtitle={
        isEdit
          ? "Atualize os dados do produto selecionado."
          : "Preencha os campos para criar um produto."
      }
    >
      <Card>
        <CardHeader className="flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <CardTitle>Dados do produto</CardTitle>
            <CardDescription>Campos obrigatorios para cadastro e manutencao.</CardDescription>
          </div>
          <Button asChild variant="outline">
            <Link to="/products">
              <ArrowLeft />
              Voltar
            </Link>
          </Button>
        </CardHeader>
        <CardContent>
          {isEdit && isProductLoading ? (
            <p className="text-sm text-muted-foreground">Carregando produto...</p>
          ) : (
            <ProductForm
              product={product}
              categories={categories ?? []}
              isCategoriesLoading={isCategoriesLoading}
              isSubmitting={isSubmitting}
              showSubmitError={createMutation.isError || updateMutation.isError}
              onSubmit={onSubmit}
            />
          )}
        </CardContent>
      </Card>
    </AppShell>
  );
}

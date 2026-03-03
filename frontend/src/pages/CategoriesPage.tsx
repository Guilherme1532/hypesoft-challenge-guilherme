import { useState } from "react";
import { Link } from "react-router-dom";
import { Edit3, FolderTree, Trash2, X } from "lucide-react";
import {
  useCreateCategoryMutation,
  useDeleteCategoryMutation,
  useUpdateCategoryMutation,
} from "@/hooks/mutations";
import { useToast } from "@/hooks/use-toast";
import { useCategories } from "@/hooks/queries";
import { hasAnyRole } from "@/services/auth";
import CategoryForm from "@/components/forms/CategoryForm";
import { categoryFormSchema, type CategoryFormValues } from "@/components/forms/schemas";
import AppShell from "@/components/layout/AppShell";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import ConfirmDialog from "@/components/ui/confirm-dialog";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

export default function CategoriesPage() {
  const canManage = hasAnyRole(["manager", "admin"]);
  const toast = useToast();
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editingName, setEditingName] = useState("");
  const [deletingCategory, setDeletingCategory] = useState<{ id: string; name: string } | null>(null);

  const { data: categories, isLoading, isError, refetch } = useCategories();
  const createMutation = useCreateCategoryMutation();
  const updateMutation = useUpdateCategoryMutation();
  const deleteMutation = useDeleteCategoryMutation();

  async function onSubmitCreate(values: CategoryFormValues) {
    try {
      await createMutation.mutateAsync(values);
      toast.success("Categoria criada", "A nova categoria foi cadastrada com sucesso.");
      return true;
    } catch {
      toast.error("Falha ao criar categoria", "Tente novamente em alguns instantes.");
      return false;
    }
  }

  async function onSaveEdit() {
    if (!editingId) return;
    const parsed = categoryFormSchema.safeParse({ name: editingName });
    if (!parsed.success) return;

    try {
      await updateMutation.mutateAsync({
        id: editingId,
        payload: parsed.data,
      });

      toast.success("Categoria atualizada", "Alteracoes salvas com sucesso.");
      setEditingId(null);
      setEditingName("");
    } catch {
      toast.error("Falha ao atualizar categoria", "Tente novamente em alguns instantes.");
    }
  }

  function startEdit(id: string, name: string) {
    setEditingId(id);
    setEditingName(name);
  }

  function cancelEdit() {
    setEditingId(null);
    setEditingName("");
  }

  async function confirmDelete() {
    if (!deletingCategory) return;

    try {
      await deleteMutation.mutateAsync(deletingCategory.id);
      toast.success("Categoria excluida", `${deletingCategory.name} foi removida.`);
      setDeletingCategory(null);
    } catch {
      toast.error(
        "Falha ao excluir categoria",
        "Essa categoria pode estar em uso por algum produto."
      );
    }
  }

  return (
    <AppShell
      activeNav="categories"
      title="Categorias"
      subtitle="Gerencie as categorias usadas no cadastro dos produtos."
    >
      <Card>
        <CardHeader className="flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <CardTitle>Nova categoria</CardTitle>
            <CardDescription>Apenas manager/admin pode criar e alterar categorias.</CardDescription>
          </div>
          <Button asChild variant="outline">
            <Link to="/products">Ir para produtos</Link>
          </Button>
        </CardHeader>
        <CardContent>
          <CategoryForm
            onSubmit={onSubmitCreate}
            disabled={!canManage}
            isSubmitting={createMutation.isPending}
          />
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <CardTitle>Lista de categorias</CardTitle>
            <CardDescription>{categories?.length ?? 0} categorias cadastradas.</CardDescription>
          </div>
          <FolderTree className="size-5 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          {isError ? (
            <div className="space-y-2">
              <p className="text-sm text-destructive">Falha ao carregar categorias.</p>
              <Button onClick={() => refetch()}>Tentar novamente</Button>
            </div>
          ) : (
            <>
              <div className="space-y-3 md:hidden">
                {isLoading ? (
                  <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                    Carregando...
                  </div>
                ) : categories?.length ? (
                  categories.map((category) => (
                    <div key={category.id} className="rounded-xl border bg-background p-4">
                      {editingId === category.id ? (
                        <Input
                          value={editingName}
                          onChange={(event) => setEditingName(event.target.value)}
                          className="mb-3"
                        />
                      ) : (
                        <p className="mb-3 font-medium">{category.name}</p>
                      )}
                      <div className="flex flex-wrap gap-2">
                        {editingId === category.id ? (
                          <>
                            <Button
                              size="sm"
                              onClick={() => void onSaveEdit()}
                              disabled={!canManage || updateMutation.isPending}
                            >
                              Salvar
                            </Button>
                            <Button size="sm" variant="outline" onClick={cancelEdit}>
                              <X />
                              Cancelar
                            </Button>
                          </>
                        ) : (
                          <Button
                            size="sm"
                            variant="outline"
                            onClick={() => startEdit(category.id, category.name)}
                            disabled={!canManage}
                          >
                            <Edit3 />
                            Editar
                          </Button>
                        )}
                        <Button
                          size="sm"
                          variant="destructive"
                          onClick={() => setDeletingCategory({ id: category.id, name: category.name })}
                          disabled={!canManage}
                        >
                          <Trash2 />
                          Excluir
                        </Button>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                    Nenhuma categoria cadastrada.
                  </div>
                )}
              </div>

              <div className="hidden md:block">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Nome</TableHead>
                      <TableHead className="text-right">Acoes</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {isLoading ? (
                      <TableRow>
                        <TableCell colSpan={2}>Carregando...</TableCell>
                      </TableRow>
                    ) : categories?.length ? (
                      categories.map((category) => (
                        <TableRow key={category.id}>
                          <TableCell>
                            {editingId === category.id ? (
                              <Input
                                value={editingName}
                                onChange={(event) => setEditingName(event.target.value)}
                                className="max-w-sm"
                              />
                            ) : (
                              category.name
                            )}
                          </TableCell>
                          <TableCell className="text-right">
                            <div className="flex flex-wrap justify-end gap-2">
                              {editingId === category.id ? (
                                <>
                                  <Button
                                    size="sm"
                                    onClick={() => void onSaveEdit()}
                                    disabled={!canManage || updateMutation.isPending}
                                  >
                                    Salvar
                                  </Button>
                                  <Button size="sm" variant="outline" onClick={cancelEdit}>
                                    <X />
                                    Cancelar
                                  </Button>
                                </>
                              ) : (
                                <Button
                                  size="sm"
                                  variant="outline"
                                  onClick={() => startEdit(category.id, category.name)}
                                  disabled={!canManage}
                                >
                                  <Edit3 />
                                  Editar
                                </Button>
                              )}
                              <Button
                                size="sm"
                                variant="destructive"
                                onClick={() =>
                                  setDeletingCategory({ id: category.id, name: category.name })
                                }
                                disabled={!canManage}
                              >
                                <Trash2 />
                                Excluir
                              </Button>
                            </div>
                          </TableCell>
                        </TableRow>
                      ))
                    ) : (
                      <TableRow>
                        <TableCell colSpan={2} className="text-muted-foreground">
                          Nenhuma categoria cadastrada.
                        </TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              </div>
            </>
          )}
        </CardContent>
      </Card>

      <ConfirmDialog
        open={Boolean(deletingCategory)}
        onOpenChange={(open) => {
          if (!open) setDeletingCategory(null);
        }}
        title="Excluir categoria?"
        description={
          deletingCategory
            ? `Essa acao removera "${deletingCategory.name}" permanentemente.`
            : "Essa acao removera a categoria permanentemente."
        }
        confirmLabel="Excluir"
        loading={deleteMutation.isPending}
        onConfirm={() => void confirmDelete()}
      />
    </AppShell>
  );
}

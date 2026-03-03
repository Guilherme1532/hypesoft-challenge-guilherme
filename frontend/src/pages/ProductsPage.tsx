import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { Edit3, PackageCheck, Plus, Search, Trash2 } from "lucide-react";
import { useDeleteProductMutation, useUpdateProductStockMutation } from "@/hooks/mutations";
import { useToast } from "@/hooks/use-toast";
import { useCategories, useProducts } from "@/hooks/queries";
import { hasAnyRole } from "@/services/auth";
import AppShell from "@/components/layout/AppShell";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import ConfirmDialog from "@/components/ui/confirm-dialog";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import type { Product } from "@/types/api";

const pageSize = 10;

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

const numberFormatter = new Intl.NumberFormat("pt-BR");

export default function ProductsPage() {
  const canManage = hasAnyRole(["manager", "admin"]);
  const toast = useToast();
  const [searchInput, setSearchInput] = useState("");
  const [search, setSearch] = useState("");
  const [categoryId, setCategoryId] = useState("");
  const [page, setPage] = useState(1);
  const [deletingProduct, setDeletingProduct] = useState<{ id: string; name: string } | null>(null);
  const [stockDialogProduct, setStockDialogProduct] = useState<Product | null>(null);
  const [stockQuantityInput, setStockQuantityInput] = useState("");
  const [selectedProductIds, setSelectedProductIds] = useState<string[]>([]);
  const [bulkStockInput, setBulkStockInput] = useState("");

  const { data: categories, isLoading: isCategoriesLoading } = useCategories();
  const deleteMutation = useDeleteProductMutation();
  const updateStockMutation = useUpdateProductStockMutation();
  const { data, isLoading, isError, refetch, isFetching } = useProducts({
    page,
    pageSize,
    search: search || undefined,
    categoryId: categoryId || undefined,
  });

  useEffect(() => {
    const timer = setTimeout(() => setSearch(searchInput.trim()), 350);
    return () => clearTimeout(timer);
  }, [searchInput]);

  const totalPages = useMemo(() => {
    if (!data?.total) return 1;
    return Math.max(1, Math.ceil(data.total / data.pageSize));
  }, [data?.total, data?.pageSize]);

  const categoryMap = useMemo(
    () => new Map(categories?.map((category) => [category.id, category.name]) ?? []),
    [categories]
  );
  const currentPageProductIds = useMemo(
    () => data?.items.map((product) => product.id) ?? [],
    [data?.items]
  );
  const selectedCurrentPageIds = useMemo(
    () => selectedProductIds.filter((id) => currentPageProductIds.includes(id)),
    [selectedProductIds, currentPageProductIds]
  );

  async function confirmDelete() {
    if (!deletingProduct) return;

    try {
      await deleteMutation.mutateAsync(deletingProduct.id);
      await refetch();
      toast.success("Produto excluido", `${deletingProduct.name} foi removido com sucesso.`);
      setDeletingProduct(null);
    } catch {
      toast.error("Falha ao excluir produto", "Tente novamente em alguns instantes.");
    }
  }

  function openStockDialog(product: Product) {
    setStockDialogProduct(product);
    setStockQuantityInput(String(product.stockQuantity));
  }

  function toggleProductSelection(id: string) {
    setSelectedProductIds((current) =>
      current.includes(id) ? current.filter((itemId) => itemId !== id) : [...current, id]
    );
  }

  function toggleCurrentPageSelection(checked: boolean) {
    if (!checked) {
      setSelectedProductIds([]);
      return;
    }

    setSelectedProductIds(data?.items.map((product) => product.id) ?? []);
  }

  async function submitStockUpdate() {
    if (!stockDialogProduct) return;

    const parsedValue = Number.parseInt(stockQuantityInput, 10);
    if (!Number.isFinite(parsedValue) || parsedValue < 0) {
      toast.error("Estoque invalido", "Informe um numero inteiro maior ou igual a zero.");
      return;
    }

    try {
      await updateStockMutation.mutateAsync({
        id: stockDialogProduct.id,
        payload: { stockQuantity: parsedValue },
      });
      await refetch();
      toast.success("Estoque atualizado", `Novo estoque de ${stockDialogProduct.name}: ${parsedValue}.`);
      setStockDialogProduct(null);
      setStockQuantityInput("");
    } catch {
      toast.error("Falha ao atualizar estoque", "Tente novamente em alguns instantes.");
    }
  }

  async function applyBulkStockUpdate() {
    const parsedValue = Number.parseInt(bulkStockInput, 10);

    if (!Number.isFinite(parsedValue) || parsedValue < 0) {
      toast.error("Estoque invalido", "Informe um numero inteiro maior ou igual a zero.");
      return;
    }

    if (!selectedCurrentPageIds.length) {
      toast.error("Nenhum produto selecionado", "Selecione ao menos um produto para atualizar.");
      return;
    }

    try {
      await Promise.all(
        selectedCurrentPageIds.map((id) =>
          updateStockMutation.mutateAsync({
            id,
            payload: { stockQuantity: parsedValue },
          })
        )
      );
      await refetch();
      toast.success(
        "Estoque em lote atualizado",
        `${selectedCurrentPageIds.length} produto(s) atualizado(s) para ${parsedValue}.`
      );
      setSelectedProductIds([]);
      setBulkStockInput("");
    } catch {
      toast.error("Falha na atualizacao em lote", "Alguns itens podem nao ter sido atualizados.");
    }
  }

  const allCurrentPageSelected =
    Boolean(currentPageProductIds.length) &&
    currentPageProductIds.every((id) => selectedProductIds.includes(id));

  return (
    <AppShell
      activeNav="products"
      title="Produtos"
      subtitle="Busca, filtro e visao geral do catalogo de produtos."
    >
      <Card>
        <CardHeader className="flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <CardTitle>Filtros</CardTitle>
            <CardDescription>Pesquise por nome e filtre por categoria.</CardDescription>
          </div>
          <div className="flex w-full flex-wrap gap-2 sm:w-auto">
            <Button asChild variant="outline">
              <Link to="/categories">Categorias</Link>
            </Button>
            {canManage ? (
              <Button asChild>
                <Link to="/products/new">
                  <Plus />
                  Novo produto
                </Link>
              </Button>
            ) : (
              <Button disabled title="Apenas manager/admin">
                <Plus />
                Novo produto
              </Button>
            )}
          </div>
        </CardHeader>
        <CardContent className="grid gap-3 md:grid-cols-2">
          <div className="relative">
            <Search className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
            <Input
              value={searchInput}
              onChange={(event) => {
                setSearchInput(event.target.value);
                setPage(1);
              }}
              placeholder="Buscar por nome..."
              className="pl-9"
            />
          </div>

          <select
            value={categoryId}
            onChange={(event) => {
              setCategoryId(event.target.value);
              setPage(1);
            }}
            className="h-9 rounded-md border border-input bg-background px-3 text-sm shadow-xs outline-none"
            disabled={isCategoriesLoading}
          >
            <option value="">Todas as categorias</option>
            {(categories ?? []).map((category) => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <CardTitle>Lista de produtos</CardTitle>
            <CardDescription>
              {isFetching ? "Atualizando resultados..." : "Resultados paginados da API."}
            </CardDescription>
          </div>
          <div className="flex w-full flex-wrap items-center justify-end gap-2 sm:w-auto">
            {canManage && (
              <>
                <Input
                  type="number"
                  min={0}
                  value={bulkStockInput}
                  onChange={(event) => setBulkStockInput(event.target.value)}
                  placeholder="Estoque em lote"
                  className="w-full sm:w-40"
                />
                <Button
                  variant="outline"
                  disabled={!selectedCurrentPageIds.length || updateStockMutation.isPending}
                  onClick={() => void applyBulkStockUpdate()}
                >
                  <PackageCheck />
                  Atualizar lote ({selectedCurrentPageIds.length})
                </Button>
              </>
            )}
            <Badge variant="secondary">{numberFormatter.format(data?.total ?? 0)} itens</Badge>
          </div>
        </CardHeader>
        <CardContent>
          {isError ? (
            <div className="space-y-3">
              <p className="text-sm text-destructive">Falha ao carregar produtos.</p>
              <Button onClick={() => refetch()}>Tentar novamente</Button>
            </div>
          ) : (
            <>
              <div className="space-y-3 md:hidden">
                {isLoading ? (
                  <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                    Carregando...
                  </div>
                ) : data?.items.length ? (
                  data.items.map((product) => (
                    <div key={product.id} className="rounded-xl border bg-background p-4">
                      <div className="mb-3 flex items-start justify-between gap-2">
                        <div className="flex items-start gap-2">
                          {canManage && (
                            <input
                              type="checkbox"
                              checked={selectedProductIds.includes(product.id)}
                              onChange={() => toggleProductSelection(product.id)}
                              className="mt-1 size-4 rounded border-zinc-300"
                            />
                          )}
                          <p className="font-medium">{product.name}</p>
                        </div>
                        <Badge variant={product.stockQuantity < 10 ? "destructive" : "secondary"}>
                          {product.stockQuantity}
                        </Badge>
                      </div>
                      <div className="space-y-1 text-sm text-muted-foreground">
                        <p>Categoria: {categoryMap.get(product.categoryId) ?? "Sem categoria"}</p>
                        <p>Preco: {currencyFormatter.format(product.price)}</p>
                      </div>
                      <div className="mt-3 flex flex-wrap gap-2">
                        {canManage ? (
                          <Button asChild size="sm" variant="outline">
                            <Link to={`/products/${product.id}/edit`}>
                              <Edit3 />
                              Editar
                            </Link>
                          </Button>
                        ) : (
                          <Button size="sm" variant="outline" disabled title="Apenas manager/admin">
                            <Edit3 />
                            Editar
                          </Button>
                        )}
                        <Button
                          size="sm"
                          variant="secondary"
                          disabled={!canManage}
                          title={!canManage ? "Apenas manager/admin" : undefined}
                          onClick={() => openStockDialog(product)}
                        >
                          <PackageCheck />
                          Estoque
                        </Button>
                        <Button
                          size="sm"
                          variant="destructive"
                          disabled={!canManage}
                          title={!canManage ? "Apenas manager/admin" : undefined}
                          onClick={() => setDeletingProduct({ id: product.id, name: product.name })}
                        >
                          <Trash2 />
                          Excluir
                        </Button>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                    Nenhum produto encontrado para os filtros atuais.
                  </div>
                )}
              </div>

              <div className="hidden md:block">
                <Table>
                  <TableHeader>
                    <TableRow>
                      {canManage && (
                        <TableHead className="w-10">
                          <input
                            type="checkbox"
                            checked={allCurrentPageSelected}
                            onChange={(event) => toggleCurrentPageSelection(event.target.checked)}
                            className="size-4 rounded border-zinc-300"
                          />
                        </TableHead>
                      )}
                      <TableHead>Nome</TableHead>
                      <TableHead>Categoria</TableHead>
                      <TableHead>Preco</TableHead>
                      <TableHead>Estoque</TableHead>
                      <TableHead className="text-right">Acoes</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {isLoading ? (
                      <TableRow>
                        <TableCell colSpan={canManage ? 6 : 5} className="text-sm text-muted-foreground">
                          Carregando...
                        </TableCell>
                      </TableRow>
                    ) : data?.items.length ? (
                      data.items.map((product) => (
                        <TableRow key={product.id}>
                          {canManage && (
                            <TableCell>
                              <input
                                type="checkbox"
                                checked={selectedProductIds.includes(product.id)}
                                onChange={() => toggleProductSelection(product.id)}
                                className="size-4 rounded border-zinc-300"
                              />
                            </TableCell>
                          )}
                          <TableCell className="font-medium">{product.name}</TableCell>
                          <TableCell>{categoryMap.get(product.categoryId) ?? "Sem categoria"}</TableCell>
                          <TableCell>{currencyFormatter.format(product.price)}</TableCell>
                          <TableCell>
                            <Badge variant={product.stockQuantity < 10 ? "destructive" : "secondary"}>
                              {product.stockQuantity}
                            </Badge>
                          </TableCell>
                          <TableCell className="text-right">
                            <div className="flex flex-wrap justify-end gap-2">
                              {canManage ? (
                                <Button asChild size="sm" variant="outline">
                                  <Link to={`/products/${product.id}/edit`}>
                                    <Edit3 />
                                    Editar
                                  </Link>
                                </Button>
                              ) : (
                                <Button size="sm" variant="outline" disabled title="Apenas manager/admin">
                                  <Edit3 />
                                  Editar
                                </Button>
                              )}
                              <Button
                                size="sm"
                                variant="secondary"
                                disabled={!canManage}
                                title={!canManage ? "Apenas manager/admin" : undefined}
                                onClick={() => openStockDialog(product)}
                              >
                                <PackageCheck />
                                Estoque
                              </Button>
                              <Button
                                size="sm"
                                variant="destructive"
                                disabled={!canManage}
                                title={!canManage ? "Apenas manager/admin" : undefined}
                                onClick={() =>
                                  setDeletingProduct({ id: product.id, name: product.name })
                                }
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
                        <TableCell colSpan={canManage ? 6 : 5} className="text-sm text-muted-foreground">
                          Nenhum produto encontrado para os filtros atuais.
                        </TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              </div>

              <div className="mt-4 flex flex-wrap items-center justify-between gap-2">
                <p className="text-sm text-muted-foreground">
                  Pagina {data?.page ?? page} de {totalPages}
                </p>
                <div className="flex w-full gap-2 sm:w-auto">
                  <Button
                    variant="outline"
                    onClick={() => setPage((prev) => Math.max(1, prev - 1))}
                    disabled={(data?.page ?? page) <= 1 || isLoading}
                    className="flex-1 sm:flex-none"
                  >
                    Anterior
                  </Button>
                  <Button
                    variant="outline"
                    onClick={() => setPage((prev) => Math.min(totalPages, prev + 1))}
                    disabled={(data?.page ?? page) >= totalPages || isLoading}
                    className="flex-1 sm:flex-none"
                  >
                    Proxima
                  </Button>
                </div>
              </div>
            </>
          )}
        </CardContent>
      </Card>

      <ConfirmDialog
        open={Boolean(deletingProduct)}
        onOpenChange={(open) => {
          if (!open) setDeletingProduct(null);
        }}
        title="Excluir produto?"
        description={
          deletingProduct
            ? `Essa acao removera "${deletingProduct.name}" permanentemente.`
            : "Essa acao removera o produto permanentemente."
        }
        confirmLabel="Excluir"
        loading={deleteMutation.isPending}
        onConfirm={() => void confirmDelete()}
      />

      <Dialog open={Boolean(stockDialogProduct)} onOpenChange={(open) => !open && setStockDialogProduct(null)}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Ajustar estoque</DialogTitle>
            <DialogDescription>
              {stockDialogProduct
                ? `Atualize a quantidade de estoque para ${stockDialogProduct.name}.`
                : "Atualize a quantidade de estoque do produto."}
            </DialogDescription>
          </DialogHeader>
          <div className="space-y-2">
            <label htmlFor="stock-update-input" className="text-sm font-medium">
              Nova quantidade
            </label>
            <Input
              id="stock-update-input"
              type="number"
              min={0}
              value={stockQuantityInput}
              onChange={(event) => setStockQuantityInput(event.target.value)}
            />
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setStockDialogProduct(null)}>
              Cancelar
            </Button>
            <Button onClick={() => void submitStockUpdate()} disabled={updateStockMutation.isPending}>
              {updateStockMutation.isPending ? "Salvando..." : "Salvar estoque"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </AppShell>
  );
}

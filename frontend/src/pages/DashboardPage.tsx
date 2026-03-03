import { useMemo } from "react";
import { AlertCircle, CircleDollarSign, TriangleAlert } from "lucide-react";
import { useCategories, useDashboardSummary } from "@/hooks/queries";
import ProductsByCategoryChart from "@/components/charts/ProductsByCategoryChart";
import AppShell from "@/components/layout/AppShell";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

const numberFormatter = new Intl.NumberFormat("pt-BR");

export default function DashboardPage() {
  const {
    data: summary,
    isLoading: isSummaryLoading,
    isError: isSummaryError,
    refetch: refetchSummary,
  } = useDashboardSummary();

  const {
    data: categories,
    isLoading: isCategoriesLoading,
    isError: isCategoriesError,
  } = useCategories();

  const categoryMap = useMemo(
    () => new Map(categories?.map((category) => [category.id, category.name]) ?? []),
    [categories]
  );

  const chartData = useMemo(
    () =>
      (summary?.productsByCategory ?? []).map((item) => ({
        category: categoryMap.get(item.categoryId) ?? "Sem categoria",
        count: item.count,
      })),
    [summary?.productsByCategory, categoryMap]
  );

  const topCategories = useMemo(
    () => [...chartData].sort((a, b) => b.count - a.count).slice(0, 5),
    [chartData]
  );

  const lowStockProducts = summary?.lowStockProducts ?? [];

  return (
    <AppShell
      activeNav="dashboard"
      title="Dashboard"
      subtitle="Overview of product and stock performance."
    >
      {isSummaryError ? (
        <Card className="border-destructive/30 bg-white">
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-destructive">
              <AlertCircle className="size-5" />
              Failed to load dashboard
            </CardTitle>
            <CardDescription>
              Could not fetch data from API. Check authentication and backend status.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <Button onClick={() => refetchSummary()}>Retry</Button>
          </CardContent>
        </Card>
      ) : (
        <>
          <section className="grid gap-4 md:grid-cols-3">
            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader className="pb-0">
                <CardDescription className="text-zinc-500">Total stock value</CardDescription>
                <CardTitle className="pt-1 md:text-2xl text-zinc-900 sm:text-4xl">
                  {isSummaryLoading ? "..." : currencyFormatter.format(summary?.totalStockValue ?? 0)}
                </CardTitle>
              </CardHeader>
              <CardContent className="pt-0">
                <Badge className="rounded-full bg-emerald-100 px-2.5 py-1 text-emerald-700 hover:bg-emerald-100">
                  +8%
                </Badge>
              </CardContent>
            </Card>

            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader className="pb-0">
                <CardDescription className="text-zinc-500">Total products</CardDescription>
                <CardTitle className="pt-1 text-3xl text-zinc-900 sm:text-4xl">
                  {isSummaryLoading ? "..." : numberFormatter.format(summary?.totalProducts ?? 0)}
                </CardTitle>
              </CardHeader>
              <CardContent className="pt-0">
                <Badge className="rounded-full bg-indigo-100 px-2.5 py-1 text-indigo-700 hover:bg-indigo-100">
                  Catalog
                </Badge>
              </CardContent>
            </Card>

            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader className="pb-0">
                <CardDescription className="text-zinc-500">Low stock items</CardDescription>
                <CardTitle className="flex items-center gap-2 pt-1 text-3xl text-zinc-900 sm:text-4xl">
                  <TriangleAlert className="size-7 text-rose-500" />
                  {isSummaryLoading ? "..." : numberFormatter.format(lowStockProducts.length)}
                </CardTitle>
              </CardHeader>
              <CardContent className="pt-0">
                <Badge className="rounded-full bg-rose-100 px-2.5 py-1 text-rose-700 hover:bg-rose-100">
                  Attention
                </Badge>
              </CardContent>
            </Card>
          </section>

          <section className="grid gap-4 xl:grid-cols-[2fr_1fr]">
            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-zinc-900">
                  <CircleDollarSign className="size-5 text-indigo-600" />
                  Products by category
                </CardTitle>
                <CardDescription>
                  {isCategoriesLoading
                    ? "Loading categories..."
                    : isCategoriesError
                      ? "Categories unavailable. Showing IDs."
                      : "Current distribution by category."}
                </CardDescription>
              </CardHeader>
              <CardContent>
                {isSummaryLoading ? (
                  <div className="h-[320px] animate-pulse rounded-xl bg-zinc-100" />
                ) : (
                  <ProductsByCategoryChart data={chartData} />
                )}
              </CardContent>
            </Card>

            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader>
                <CardTitle className="text-zinc-900">Top categories</CardTitle>
                <CardDescription>Sorted by product quantity.</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-3">
                  {isSummaryLoading ? (
                    <>
                      <div className="h-11 animate-pulse rounded-lg bg-zinc-100" />
                      <div className="h-11 animate-pulse rounded-lg bg-zinc-100" />
                      <div className="h-11 animate-pulse rounded-lg bg-zinc-100" />
                    </>
                  ) : topCategories.length ? (
                    topCategories.map((item) => (
                      <div
                        key={item.category}
                        className="flex items-center justify-between rounded-lg border border-zinc-100 px-3 py-2.5"
                      >
                        <span className="text-sm font-medium text-zinc-700">{item.category}</span>
                        <Badge variant="secondary">{item.count}</Badge>
                      </div>
                    ))
                  ) : (
                    <p className="text-sm text-zinc-500">No categories to display.</p>
                  )}
                </div>
              </CardContent>
            </Card>
          </section>

          <section className="grid gap-4 xl:grid-cols-[2fr_1fr]">
            <Card className="border-0 bg-white py-5 shadow-sm">
              <CardHeader>
                <CardTitle className="text-zinc-900">Low stock products</CardTitle>
                <CardDescription>Products requiring replenishment.</CardDescription>
              </CardHeader>
              <CardContent className="overflow-auto">
                <div className="space-y-2 md:hidden">
                  {isSummaryLoading ? (
                    <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                      Loading...
                    </div>
                  ) : lowStockProducts.length ? (
                    lowStockProducts.map((product) => (
                      <div
                        key={product.id}
                        className="flex items-center justify-between rounded-lg border px-3 py-2.5"
                      >
                        <p className="text-sm font-medium text-zinc-800">{product.name}</p>
                        <Badge variant="destructive">{product.stockQuantity} pcs</Badge>
                      </div>
                    ))
                  ) : (
                    <div className="rounded-lg border bg-muted/30 p-4 text-sm text-muted-foreground">
                      No low-stock products.
                    </div>
                  )}
                </div>

                <div className="hidden md:block">
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Product</TableHead>
                        <TableHead className="text-right">Stock</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {isSummaryLoading ? (
                        <TableRow>
                          <TableCell colSpan={2} className="text-sm text-zinc-500">
                            Loading...
                          </TableCell>
                        </TableRow>
                      ) : lowStockProducts.length ? (
                        lowStockProducts.map((product) => (
                          <TableRow key={product.id}>
                            <TableCell className="font-medium text-zinc-800">{product.name}</TableCell>
                            <TableCell className="text-right">
                              <Badge variant="destructive">{product.stockQuantity} pcs</Badge>
                            </TableCell>
                          </TableRow>
                        ))
                      ) : (
                        <TableRow>
                          <TableCell colSpan={2} className="text-sm text-zinc-500">
                            No low-stock products.
                          </TableCell>
                        </TableRow>
                      )}
                    </TableBody>
                  </Table>
                </div>
              </CardContent>
            </Card>

            <Card className="border-0 bg-gradient-to-br from-indigo-600 to-violet-500 py-5 text-white shadow-sm">
              <CardHeader>
                <CardTitle>Stock health</CardTitle>
                <CardDescription className="text-indigo-100">
                  Quick status of current inventory.
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-3">
                <div className="rounded-xl bg-white/12 p-3">
                  <p className="text-xs uppercase tracking-wide text-indigo-100">Critical items</p>
                  <p className="text-2xl font-semibold">{lowStockProducts.length}</p>
                </div>
                <div className="rounded-xl bg-white/12 p-3">
                  <p className="text-xs uppercase tracking-wide text-indigo-100">Total products</p>
                  <p className="text-2xl font-semibold">
                    {numberFormatter.format(summary?.totalProducts ?? 0)}
                  </p>
                </div>
                <Button
                  variant="secondary"
                  className="w-full rounded-xl bg-white text-indigo-700 hover:bg-white/90"
                  onClick={() => refetchSummary()}
                >
                  Refresh dashboard
                </Button>
              </CardContent>
            </Card>
          </section>
        </>
      )}
    </AppShell>
  );
}

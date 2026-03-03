import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

interface ProductsByCategoryChartProps {
  data: Array<{
    category: string;
    count: number;
  }>;
}

export default function ProductsByCategoryChart({
  data,
}: ProductsByCategoryChartProps) {
  if (!data.length) {
    return (
      <div className="flex h-75 items-center justify-center text-sm text-muted-foreground">
        Sem dados para o grafico.
      </div>
    );
  }

  return (
    <div className="h-75 w-full">
      <ResponsiveContainer width="100%" height="100%">
        <BarChart data={data} margin={{ top: 8, right: 8, bottom: 8, left: 8 }}>
          <CartesianGrid strokeDasharray="3 3" strokeOpacity={0.3} />
          <XAxis
            dataKey="category"
            tick={{ fontSize: 12 }}
            angle={-20}
            textAnchor="end"
            interval={0}
            height={60}
          />
          <YAxis allowDecimals={false} />
          <Tooltip />
          <Bar dataKey="count" fill="var(--color-chart-1)" radius={[6, 6, 0, 0]} />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
}

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { categoryFormSchema, type CategoryFormValues } from "@/components/forms/schemas";

interface CategoryFormProps {
  disabled?: boolean;
  isSubmitting?: boolean;
  onSubmit: (values: CategoryFormValues) => Promise<boolean>;
}

export default function CategoryForm({
  disabled = false,
  isSubmitting = false,
  onSubmit,
}: CategoryFormProps) {
  const form = useForm<CategoryFormValues>({
    resolver: zodResolver(categoryFormSchema),
    defaultValues: { name: "" },
  });

  async function handleSubmit(values: CategoryFormValues) {
    const success = await onSubmit(values);
    if (success) {
      form.reset({ name: "" });
    }
  }

  return (
    <form className="flex flex-wrap gap-2" onSubmit={form.handleSubmit((values) => void handleSubmit(values))}>
      <Input
        {...form.register("name")}
        placeholder="Nome da categoria"
        className="max-w-sm"
        disabled={disabled || isSubmitting}
      />
      <Button type="submit" disabled={disabled || isSubmitting}>
        <Plus />
        Criar
      </Button>
      {form.formState.errors.name && (
        <p className="w-full text-sm text-destructive">{form.formState.errors.name.message}</p>
      )}
    </form>
  );
}

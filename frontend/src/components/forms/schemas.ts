import { z } from "zod";

export const productSchema = z.object({
  name: z.string().trim().min(2, "Nome deve ter pelo menos 2 caracteres."),
  description: z.string().trim().min(2, "Descricao deve ter pelo menos 2 caracteres."),
  price: z.number().positive("Preco deve ser maior que zero."),
  categoryId: z.string().trim().min(1, "Categoria e obrigatoria."),
  stockQuantity: z.number().int().min(0, "Estoque deve ser um inteiro >= 0."),
});

export type ProductFormValues = z.infer<typeof productSchema>;

export const categoryFormSchema = z.object({
  name: z.string().trim().min(2, "Nome deve ter pelo menos 2 caracteres."),
});

export type CategoryFormValues = z.infer<typeof categoryFormSchema>;

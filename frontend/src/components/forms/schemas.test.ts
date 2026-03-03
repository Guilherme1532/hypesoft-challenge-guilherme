import { categoryFormSchema, productSchema } from "@/components/forms/schemas";

describe("form schemas", () => {
  it("accepts a valid product payload", () => {
    const parsed = productSchema.safeParse({
      name: "Mouse",
      description: "RGB",
      price: 199.9,
      categoryId: "cat-1",
      stockQuantity: 12,
    });

    expect(parsed.success).toBe(true);
  });

  it("rejects invalid product payload", () => {
    const parsed = productSchema.safeParse({
      name: "",
      description: "",
      price: -1,
      categoryId: "",
      stockQuantity: -2,
    });

    expect(parsed.success).toBe(false);
  });

  it("validates category payload", () => {
    expect(categoryFormSchema.safeParse({ name: "Acessorios" }).success).toBe(true);
    expect(categoryFormSchema.safeParse({ name: " " }).success).toBe(false);
  });
});

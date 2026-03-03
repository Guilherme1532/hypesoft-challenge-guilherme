import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";
import CategoryForm from "@/components/forms/CategoryForm";

describe("CategoryForm", () => {
  it("submits valid data and clears input on success", async () => {
    const user = userEvent.setup();
    const onSubmit = vi.fn().mockResolvedValue(true);

    render(<CategoryForm onSubmit={onSubmit} />);

    const input = screen.getByPlaceholderText("Nome da categoria");
    await user.type(input, "Perifericos");
    await user.click(screen.getByRole("button", { name: /criar/i }));

    expect(onSubmit).toHaveBeenCalledWith({ name: "Perifericos" });
    expect(input).toHaveValue("");
  });
});

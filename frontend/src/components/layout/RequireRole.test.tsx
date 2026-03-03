import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { vi } from "vitest";
import RequireRole from "@/components/layout/RequireRole";

const hasAnyRoleMock = vi.fn();

vi.mock("@/services/auth", () => ({
  hasAnyRole: (...args: unknown[]) => hasAnyRoleMock(...args),
}));

describe("RequireRole", () => {
  it("redirects to unauthorized when user lacks role", () => {
    hasAnyRoleMock.mockReturnValue(false);

    render(
      <MemoryRouter>
        <RequireRole roles={["admin"]}>
          <div>Somente admin</div>
        </RequireRole>
      </MemoryRouter>
    );

    expect(screen.queryByText("Somente admin")).not.toBeInTheDocument();
  });

  it("renders children when role is allowed", () => {
    hasAnyRoleMock.mockReturnValue(true);

    render(
      <MemoryRouter>
        <RequireRole roles={["admin"]}>
          <div>Somente admin</div>
        </RequireRole>
      </MemoryRouter>
    );

    expect(screen.getByText("Somente admin")).toBeInTheDocument();
  });
});

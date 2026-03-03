import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { vi } from "vitest";
import RequireAuth from "@/components/layout/RequireAuth";

vi.mock("@/lib/keycloak", () => ({
  keycloak: {
    authenticated: false,
  },
}));

describe("RequireAuth", () => {
  it("redirects to login when not authenticated", () => {
    render(
      <MemoryRouter>
        <RequireAuth>
          <div>Conteudo privado</div>
        </RequireAuth>
      </MemoryRouter>
    );

    expect(screen.queryByText("Conteudo privado")).not.toBeInTheDocument();
  });
});

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Hypesoft.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Keycloak:Authority"];
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role, // o ASP.NET casa RequireRole aqui
                NameClaimType = "preferred_username"
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = ctx =>
                {
                    var identity = (ClaimsIdentity?)ctx.Principal?.Identity;
                    if (identity is null) return Task.CompletedTask;

                    // 1) tenta pegar "roles" (se o mapper do Keycloak estiver certo)
                    var roleClaims = ctx.Principal!.FindAll("roles").Select(c => c.Value).ToList();

                    // 2) se vier tudo “embutido” em JSON (às vezes vem assim), tenta parsear
                    if (roleClaims.Count == 1 && roleClaims[0].StartsWith("["))
                    {
                        try
                        {
                            var arr = JsonSerializer.Deserialize<string[]>(roleClaims[0]);
                            if (arr is not null) roleClaims = arr.ToList();
                        }
                        catch { /* ignora */ }
                    }

                    // 3) fallback: realm_access.roles
                    if (roleClaims.Count == 0)
                    {
                        var realmAccess = ctx.Principal.FindFirst("realm_access")?.Value;
                        if (!string.IsNullOrWhiteSpace(realmAccess))
                        {
                            try
                            {
                                using var doc = JsonDocument.Parse(realmAccess);
                                if (doc.RootElement.TryGetProperty("roles", out var rolesEl) && rolesEl.ValueKind == JsonValueKind.Array)
                                {
                                    roleClaims = rolesEl.EnumerateArray().Select(r => r.GetString()!).Where(s => s is not null).ToList();
                                }
                            }
                            catch { /* ignora */ }
                        }
                    }

                    // adiciona como ClaimTypes.Role (o que o RequireRole usa)
                    foreach (var r in roleClaims.Distinct())
                        identity.AddClaim(new Claim(ClaimTypes.Role, r));

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("UserOnly", p =>
                p.RequireRole("user", "manager", "admin"));

            options.AddPolicy("ManagerOnly", p =>
                p.RequireRole("manager", "admin"));

            options.AddPolicy("AdminOnly", p =>
                p.RequireRole("admin"));
        });

        return services;
    }

    public static WebApplication UseApiAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
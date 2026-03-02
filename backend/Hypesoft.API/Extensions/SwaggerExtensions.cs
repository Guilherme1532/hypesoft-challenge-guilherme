using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Hypesoft.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hypesoft API",
                Version = "v1"
            });

            // Define o esquema Bearer (isso faz aparecer o botão "Authorize" no Swagger)
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Cole aqui: Bearer {seu_token}"
            });

            // Marca as rotas como protegidas (gera o cadeado)
            c.AddSecurityRequirement(document =>
            {
                var scheme = new OpenApiSecuritySchemeReference("Bearer", document, null);

                var requirement = new OpenApiSecurityRequirement();
                requirement.Add(scheme, new List<string>());

                return requirement;
            });
        });

        return services;
    }

    public static WebApplication UseApiSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
using Hypesoft.Infrastructure.Configurations;
using Hypesoft.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Repositories;

namespace Hypesoft.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind Mongo settings
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));

        // Register DbContext using EF Core Mongo Provider
        services.AddDbContext<HypesoftDbContext>((sp, options) =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;

            options.UseMongoDB(
                settings.ConnectionString,
                settings.DatabaseName);
        });
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
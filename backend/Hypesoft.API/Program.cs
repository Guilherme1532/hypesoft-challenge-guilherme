using Hypesoft.Infrastructure;
using Hypesoft.Application;
using Hypesoft.API.Middlewares;
using Hypesoft.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:3000"];

        policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddApiSwagger();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApiRateLimiting();
builder.Services.AddApiHealthChecks();

var app = builder.Build();
await app.SeedDatabaseAsync();

app.UseApiSwagger();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseApiRateLimiting();
app.UseApiHealthChecks();
app.UseCors("Frontend");

app.UseApiAuthentication();

app.MapControllers();

app.Run();

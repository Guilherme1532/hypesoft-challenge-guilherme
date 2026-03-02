using Hypesoft.Infrastructure;
using Hypesoft.Application;
using Hypesoft.API.Middlewares;
using Hypesoft.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();
builder.Services.AddApiAuthentication(builder.Configuration);

// Controllers + Swagger (template)
builder.Services.AddControllers();
builder.Services.AddApiSwagger();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApiRateLimiting();
builder.Services.AddApiHealthChecks();

var app = builder.Build();

app.UseApiSwagger();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseApiRateLimiting();
app.UseApiHealthChecks();

app.UseApiAuthentication();

app.MapControllers();

app.Run();
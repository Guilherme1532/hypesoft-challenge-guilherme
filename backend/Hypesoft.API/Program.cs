using Hypesoft.Infrastructure;
using Hypesoft.Application;
using Hypesoft.API.Middlewares;
using Hypesoft.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger (template)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddApiRateLimiting();

var app = builder.Build();

// Swagger (template)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseApiRateLimiting();
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
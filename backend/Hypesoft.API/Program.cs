using Hypesoft.Infrastructure;
using Hypesoft.Application;
using Hypesoft.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger (template)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Swagger (template)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
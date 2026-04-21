using LaRicaNoche.Api.Extensions;
using LaRicaNoche.Api.Config;
using LaRicaNoche.Api.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS (Base de Datos, Servicios, etc) ---
builder.Services.AddApplicationServices(builder.Configuration);

// Activa el mapeo automático de Mapster que creamos
MappingConfig.Configure();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- ACTIVACIÓN DEL MIDDLEWARE (ESCUDO DE ERRORES) ---
// Debe ir al inicio para atrapar cualquier error en la API
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Mapeo de rutas de controladores para que Scalar las vea
app.MapControllers();

app.Run();

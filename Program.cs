using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using Scalar.AspNetCore;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<LaRicaNocheDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// NLua
builder.Services.AddSingleton<ILuaService, LuaService>();

// Controladores de la API
builder.Services.AddScoped<ITipoHabitacionService, TipoHabitacionService>();
builder.Services.AddScoped<IHabitacionService, HabitacionService>();

//  Controladores y OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // /scalar/v1
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
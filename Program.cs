using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<LaRicaNocheDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// NLua
builder.Services.AddSingleton<ILuaService, LuaService>();

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
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Services.Interfaces.lua;
using LaRicaNoche.Api.Services.Implementations.lua;

namespace LaRicaNoche.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Conexión a SQL Server
        services.AddDbContext<LaRicaNocheDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registro de Mapster
        var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);

        // Servicio de ejecucion de scripts LUA
        services.AddScoped<ILuaService, LuaService>();

        // Servicios de las habitaciones
        services.AddScoped<IHabitacionService, HabitacionService>();

        // Servicio de usuarios
        services.AddScoped<IUsuarioService, UsuarioService>();

        // Servicio de clientes
        services.AddScoped<IClienteService, ClienteService>();

        // Servicio de reservas
        services.AddScoped<IReservaService, ReservaService>();

        // Servicio de categorías
        services.AddScoped<ICategoriaService, CategoriaService>();

        // Servicio de productos
        services.AddScoped<IProductoService, ProductoService>();

        // Servicio de ventas
        services.AddScoped<IVentaService, VentaService>();

        // Servicio de comprobantes
        services.AddScoped<IComprobanteService, ComprobanteService>();

        // Servicio de reportes
        services.AddScoped<IReporteService, ReporteService>();

        return services;
    }
}

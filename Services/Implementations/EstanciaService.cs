using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class EstanciaService : IEstanciaService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<EstanciaService> _logger;

    public EstanciaService(HotelDbContext db, ILogger<EstanciaService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<Estancia>> GetAllAsync()
    {
        return await _db.Estancias
            .Include(e => e.Habitacion!).ThenInclude(h => h.Tipo)
            .Include(e => e.ClienteTitular)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Estancia?> GetByIdAsync(int id)
    {
        return await _db.Estancias
            .Include(e => e.Habitacion!).ThenInclude(h => h.Tipo)
            .Include(e => e.ClienteTitular)
            .Include(e => e.ItemsEstancia!).ThenInclude(i => i.Producto)
            .Include(e => e.Huespedes!).ThenInclude(h => h.Cliente)
            .FirstOrDefaultAsync(e => e.IdEstancia == id);
    }

    public async Task<Estancia> CreateAsync(Estancia estancia)
    {
        var habitacion = await _db.Habitaciones
            .Include(h => h.Estado)
            .FirstOrDefaultAsync(h => h.IdHabitacion == estancia.IdHabitacion);

        if (habitacion == null)
            throw new ArgumentException("Habitación no encontrada.");

        if (habitacion.IdEstado != 1 && habitacion.IdEstado != 5)
            throw new InvalidOperationException($"La habitación {habitacion.NumeroHabitacion} no está disponible.");

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.Estancias.Add(estancia);
            await _db.SaveChangesAsync(); // para obtener IdEstancia

            // Cambiar estado de habitación a "Ocupada"
            habitacion.IdEstado = 2; // Ocupada
            habitacion.FechaUltimoCambio = DateTime.UtcNow;

            _logger.LogInformation("Check-in realizado: Estancia {Id}, Habitación {Numero}", estancia.IdEstancia, habitacion.NumeroHabitacion);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return estancia;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Estancia?> CheckoutAsync(int idEstancia, int idUsuario)
    {
        var estancia = await _db.Estancias
            .Include(e => e.Habitacion)
            .FirstOrDefaultAsync(e => e.IdEstancia == idEstancia);

        if (estancia == null || estancia.FechaCheckoutReal != null)
            return null;

        estancia.FechaCheckoutReal = DateTime.UtcNow;
        estancia.Estado = "Finalizada";

        // Liberar habitación a "Limpieza" (id 3)
        if (estancia.Habitacion != null)
        {
            estancia.Habitacion.IdEstado = 3; // Limpieza
            estancia.Habitacion.FechaUltimoCambio = DateTime.UtcNow;
            estancia.Habitacion.UsuarioCambio = idUsuario;

            _db.HistorialEstadoHabitaciones.Add(new HistorialEstadoHabitacion
            {
                IdHabitacion = estancia.Habitacion.IdHabitacion,
                IdEstadoAnterior = 2, // Ocupada
                IdEstadoNuevo = 3,    // Limpieza
                FechaCambio = DateTime.UtcNow,
                IdUsuario = idUsuario
            });
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Check-out realizado: Estancia {Id}, Habitación {Numero}", idEstancia, estancia.Habitacion?.NumeroHabitacion);
        return estancia;
    }

    public async Task<bool> AddHuespedAsync(int idEstancia, Huesped huesped)
    {
        var estancia = await _db.Estancias.FindAsync(idEstancia);
        if (estancia == null) return false;

        huesped.IdEstancia = idEstancia;
        _db.Huespedes.Add(huesped);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Huésped {IdCliente} añadido a estancia {IdEstancia}", huesped.IdCliente, idEstancia);
        return true;
    }

    public async Task<bool> AddConsumoAsync(int idEstancia, ItemEstancia item)
    {
        var estancia = await _db.Estancias.FindAsync(idEstancia);
        if (estancia == null) return false;

        item.IdEstancia = idEstancia;
        _db.ItemsEstancia.Add(item);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Consumo añadido a estancia {IdEstancia}: Producto {IdProducto}", idEstancia, item.IdProducto);
        return true;
    }
}

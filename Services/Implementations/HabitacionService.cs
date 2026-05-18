using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class HabitacionService : IHabitacionService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<HabitacionService> _logger;

    public HabitacionService(HotelDbContext db, ILogger<HabitacionService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<Habitacion>> GetAllAsync()
    {
        return await _db.Habitaciones
            .Include(h => h.Tipo)
            .Include(h => h.Estado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Habitacion?> GetByIdAsync(int id)
    {
        return await _db.Habitaciones
            .Include(h => h.Tipo)
            .Include(h => h.Estado)
            .FirstOrDefaultAsync(h => h.IdHabitacion == id);
    }

    public async Task<Habitacion> CreateAsync(Habitacion habitacion)
    {
        _db.Habitaciones.Add(habitacion);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Habitación {Numero} creada", habitacion.NumeroHabitacion);
        return habitacion;
    }

    public async Task<Habitacion?> UpdateAsync(int id, Habitacion habitacionActualizada)
    {
        var existente = await _db.Habitaciones.FindAsync(id);
        if (existente == null) return null;

        _db.Entry(existente).CurrentValues.SetValues(habitacionActualizada);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Habitación {Numero} actualizada", existente.NumeroHabitacion);
        return existente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var habitacion = await _db.Habitaciones.FindAsync(id);
        if (habitacion == null) return false;

        _db.Habitaciones.Remove(habitacion);
        await _db.SaveChangesAsync();
        _logger.LogWarning("Habitación {Numero} eliminada", habitacion.NumeroHabitacion);
        return true;
    }

    public async Task<bool> CambiarEstadoAsync(int idHabitacion, int idNuevoEstado, int idUsuario, string? observacion = null)
    {
        var habitacion = await _db.Habitaciones
            .Include(h => h.Estado)
            .FirstOrDefaultAsync(h => h.IdHabitacion == idHabitacion);

        if (habitacion == null) return false;

        var estadoAnterior = habitacion.IdEstado;
        var estadoNuevo = await _db.EstadosHabitacion.FindAsync(idNuevoEstado);
        if (estadoNuevo == null) return false;

        var transicionValida = await _db.TransicionesEstado
            .AnyAsync(t => t.IdEstadoActual == estadoAnterior && t.IdEstadoSiguiente == idNuevoEstado);

        if (!transicionValida)
        {
            _logger.LogWarning("Transición de estado no permitida: {Anterior} -> {Nuevo} en habitación {Id}",
                estadoAnterior, idNuevoEstado, idHabitacion);
            throw new InvalidOperationException($"No se puede cambiar de estado '{estadoAnterior}' a '{idNuevoEstado}'.");
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            habitacion.IdEstado = idNuevoEstado;
            habitacion.FechaUltimoCambio = DateTime.UtcNow;
            habitacion.UsuarioCambio = idUsuario;

            var historial = new HistorialEstadoHabitacion
            {
                IdHabitacion = idHabitacion,
                IdEstadoAnterior = estadoAnterior,
                IdEstadoNuevo = idNuevoEstado,
                FechaCambio = DateTime.UtcNow,
                IdUsuario = idUsuario,
                Observacion = observacion
            };

            _db.HistorialEstadoHabitaciones.Add(historial);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Habitación {Id} cambió de estado {Anterior} a {Nuevo}", idHabitacion, estadoAnterior, idNuevoEstado);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al cambiar estado de habitación {Id}", idHabitacion);
            throw;
        }
    }
}

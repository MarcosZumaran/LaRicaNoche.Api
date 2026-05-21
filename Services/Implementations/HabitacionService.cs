using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Response;
using HotelGenericoApi.Hubs;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class HabitacionService : IHabitacionService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<HabitacionService> _logger;
    private readonly IHubContext<HabitacionHub> _hubContext;

    public HabitacionService(HotelDbContext db, ILogger<HabitacionService> logger, IHubContext<HabitacionHub> hubContext)
    {
        _db = db;
        _logger = logger;
        _hubContext = hubContext;
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

            // Enviar notificación en tiempo real a todos los clientes conectados
            await _hubContext.Clients.All.SendAsync("EstadoHabitacionCambiado", new
            {
                idHabitacion,
                numero = habitacion.NumeroHabitacion,
                nuevoEstado = estadoNuevo.Nombre
            });

            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al cambiar estado de habitación {Id}", idHabitacion);
            throw;
        }
    }

    public async Task<List<HabitacionEstadoActualDto>> GetEstadoActualAsync()
    {
        var habitaciones = await _db.Habitaciones
            .Include(h => h.Tipo)
            .Include(h => h.Estado)
            .Include(h => h.Estancias.Where(e => e.FechaCheckoutReal == null))
                .ThenInclude(e => e.ClienteTitular)
            .AsNoTracking()
            .ToListAsync();

        // Cargar transiciones permitidas
        var transiciones = await _db.TransicionesEstado.ToListAsync();

        // Estados
        var estados = await _db.EstadosHabitacion
            .ToDictionaryAsync(e => e.IdEstado, e => e.Nombre);

        // Reservas activas
        var idsHabitacion = habitaciones.Select(h => h.IdHabitacion).ToList();
        var reservasActivas = await _db.Reservas
            .Where(r => idsHabitacion.Contains(r.IdHabitacion) && r.Estado == "Confirmada")
            .GroupBy(r => r.IdHabitacion)
            .ToDictionaryAsync(g => g.Key, g => g.First());

        return habitaciones.Select(h =>
        {
            var estanciaActiva = h.Estancias.FirstOrDefault(e => e.FechaCheckoutReal == null);

            var acciones = new List<string>();

            foreach (var t in transiciones.Where(t => t.IdEstadoActual == h.IdEstado))
            {
                if (t.IdEstadoSiguiente == 2 && !reservasActivas.ContainsKey(h.IdHabitacion))
                    acciones.Add("CheckIn");
                else if (t.IdEstadoSiguiente == 2 && reservasActivas.ContainsKey(h.IdHabitacion))
                    acciones.Add("CheckIn");
                else if (t.IdEstadoActual == 2 && t.IdEstadoSiguiente == 3)
                    acciones.Add("CheckOut");
                else if (t.IdEstadoSiguiente == 4)
                    acciones.Add("Mantenimiento");
                else if (t.IdEstadoSiguiente == 1)
                    acciones.Add("Habilitar");
                else if (t.IdEstadoSiguiente == 5)
                    acciones.Add("Reservar");
            }

            if (reservasActivas.ContainsKey(h.IdHabitacion))
                acciones.Add("CancelarReserva");

            return new HabitacionEstadoActualDto(
                IdHabitacion: h.IdHabitacion,
                NumeroHabitacion: h.NumeroHabitacion,
                Piso: h.Piso,
                IdTipo: h.IdTipo,
                NombreTipo: h.Tipo?.Nombre ?? "",
                PrecioNoche: h.PrecioNoche,
                IdEstado: h.IdEstado,
                NombreEstado: h.Estado?.Nombre ?? "",
                Descripcion: h.Descripcion,
                IdEstanciaActiva: estanciaActiva?.IdEstancia,
                ClienteHuesped: estanciaActiva?.ClienteTitular != null
                    ? $"{estanciaActiva.ClienteTitular.Nombres} {estanciaActiva.ClienteTitular.Apellidos}"
                    : null,
                AccionesDisponibles: acciones,
                FechaCheckin: estanciaActiva?.FechaCheckin,
                FechaCheckoutPrevista: estanciaActiva?.FechaCheckoutPrevista,
                FechaReservaEntrada: null
            );
        }).ToList();
    }

    public async Task<List<HabitacionEstadoActualDto>> GetDisponiblesAsync(DateTime fechaEntrada, DateTime fechaSalida)
    {
        var todas = await GetEstadoActualAsync();
        var idsOcupadas = await _db.Reservas
            .Where(r => r.Estado != "Cancelada" &&
                        r.FechaEntradaPrevista < fechaSalida &&
                        r.FechaSalidaPrevista > fechaEntrada)
            .Select(r => r.IdHabitacion)
            .Union(_db.Estancias
                .Where(e => e.Estado == "Activa" &&
                            e.FechaCheckin < fechaSalida &&
                            e.FechaCheckoutPrevista > fechaEntrada)
                .Select(e => e.IdHabitacion))
            .ToListAsync();

        return todas.Where(h => !idsOcupadas.Contains(h.IdHabitacion)).ToList();
    }
}

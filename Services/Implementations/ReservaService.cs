using Microsoft.EntityFrameworkCore;
using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Response;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class ReservaService : IReservaService
{
    private readonly HotelDbContext _db;

    public ReservaService(HotelDbContext db)
    {
        _db = db;
    }

    public async Task<List<ReservaResponseDto>> GetAllAsync()
    {
        return await _db.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Habitacion)
            .Select(r => new ReservaResponseDto(
                r.IdReserva,
                r.IdHabitacion,
                r.Habitacion != null ? r.Habitacion.NumeroHabitacion : null,
                r.Cliente != null ? $"{r.Cliente.Nombres} {r.Cliente.Apellidos}" : null,
                r.FechaEntradaPrevista,
                r.FechaSalidaPrevista,
                r.MontoTotal,
                r.Estado ?? "Pendiente",
                r.Cliente != null ? r.Cliente.Documento : null,
                r.Observaciones,
                r.EsNoShow
            ))
            .ToListAsync();
    }
}

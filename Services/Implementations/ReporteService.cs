using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class ReporteService : IReporteService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<ReporteService> _logger;

    public ReporteService(HotelDbContext db, ILogger<ReporteService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<VCierreCajaDiario>> GetCierreCajaAsync(DateOnly fecha)
    {
        return await _db.VCierreCajaDiario
            .Where(c => c.Fecha == fecha)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<VEstadoHabitacion>> GetEstadoHabitacionesAsync()
    {
        return await _db.VEstadoHabitacion
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<VOcupacionDiaria>> GetOcupacionDiariaAsync(DateOnly fecha)
    {
        return await _db.VOcupacionDiaria
            .Where(o => o.Fecha == fecha)
            .AsNoTracking()
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Response;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class ComprobanteService : IComprobanteService
{
    private readonly LaRicaNocheDbContext _db;

    public ComprobanteService(LaRicaNocheDbContext db) => _db = db;

    public async Task<IEnumerable<ComprobanteResponseDto>> GetAllAsync()
    {
        return await _db.Comprobantes
            .Include(c => c.IdEstadoSunatNavigation)
            .AsNoTracking()
            .Select(c => new ComprobanteResponseDto(
                c.IdComprobante, c.IdEstancia, c.IdVenta,
                c.TipoComprobante, c.Serie, c.Correlativo,
                c.FechaEmision, c.MontoTotal, c.IgvMonto,
                c.ClienteDocumentoTipo, c.ClienteDocumentoNum,
                c.ClienteNombre, c.MetodoPago,
                c.IdEstadoSunat,
                c.IdEstadoSunatNavigation != null ? c.IdEstadoSunatNavigation.Descripcion : null,
                c.FechaEnvio, c.IntentosEnvio
            )).ToListAsync();
    }

    public async Task<ComprobanteResponseDto?> GetByIdAsync(int id)
    {
        var c = await _db.Comprobantes
            .Include(c => c.IdEstadoSunatNavigation)
            .FirstOrDefaultAsync(x => x.IdComprobante == id);

        if (c is null) return null;

        return new ComprobanteResponseDto(
            c.IdComprobante, c.IdEstancia, c.IdVenta,
            c.TipoComprobante, c.Serie, c.Correlativo,
            c.FechaEmision, c.MontoTotal, c.IgvMonto,
            c.ClienteDocumentoTipo, c.ClienteDocumentoNum,
            c.ClienteNombre, c.MetodoPago,
            c.IdEstadoSunat, c.IdEstadoSunatNavigation?.Descripcion,
            c.FechaEnvio, c.IntentosEnvio
        );
    }

    public async Task<bool> MarcarComoEnviadoAsync(int id, string hashXml)
    {
        var entity = await _db.Comprobantes.FindAsync(id);
        if (entity is null) return false;

        entity.IdEstadoSunat = 2; // Enviado
        entity.FechaEnvio = DateTime.UtcNow;
        entity.IntentosEnvio = (entity.IntentosEnvio ?? 0) + 1;
        entity.HashXml = hashXml;
        await _db.SaveChangesAsync();
        return true;
    }
}
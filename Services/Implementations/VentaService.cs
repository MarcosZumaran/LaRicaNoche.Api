using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelGenericoApi.Data;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Services.Implementations;

public class VentaService : IVentaService
{
    private readonly HotelDbContext _db;
    private readonly ILogger<VentaService> _logger;

    public VentaService(HotelDbContext db, ILogger<VentaService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<Venta>> GetAllAsync()
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.MetodoPagoNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.MetodoPagoNavigation)
            .Include(v => v.ItemsVenta!).ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(v => v.IdVenta == id);
    }

    public async Task<Venta> CreateAsync(Venta venta)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            venta.FechaVenta = DateTime.UtcNow;
            _db.Ventas.Add(venta);
            await _db.SaveChangesAsync(); // para obtener IdVenta

            if (venta.ItemsVenta != null && venta.ItemsVenta.Any())
            {
                foreach (var item in venta.ItemsVenta)
                {
                    item.IdVenta = venta.IdVenta;
                    _db.ItemsVenta.Add(item);
                }
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Venta {Id} creada por {MetodoPago}, total {Total}", venta.IdVenta, venta.MetodoPago, venta.Total);
            return venta;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var venta = await _db.Ventas.FindAsync(id);
        if (venta == null) return false;

        _db.Ventas.Remove(venta);
        await _db.SaveChangesAsync();
        _logger.LogWarning("Venta {Id} eliminada", id);
        return true;
    }
}

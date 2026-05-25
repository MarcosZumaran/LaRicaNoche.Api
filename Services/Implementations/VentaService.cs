using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelGenericoApi.Data;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;
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

    public async Task<List<VentaResponseDto>> GetAllAsync()
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.MetodoPagoNavigation)
            .Include(v => v.ItemsVenta!).ThenInclude(i => i.Producto)
            .AsNoTracking()
            .OrderByDescending(v => v.FechaVenta)
            .Select(v => new VentaResponseDto
            {
                IdVenta = v.IdVenta,
                IdCliente = v.IdCliente,
                ClienteNombre = v.Cliente != null
                    ? $"{v.Cliente.Nombres} {v.Cliente.Apellidos}"
                    : null,
                FechaVenta = v.FechaVenta,
                Total = v.Total,
                MetodoPago = v.MetodoPagoNavigation != null
                    ? v.MetodoPagoNavigation.Descripcion
                    : v.MetodoPago,
                Items = v.ItemsVenta!.Select(i => new ItemVentaResponseDto
                {
                    IdItem = i.IdItem,
                    IdProducto = i.IdProducto,
                    NombreProducto = i.Producto != null ? i.Producto.Nombre : null,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    Subtotal = i.Subtotal ?? (i.PrecioUnitario * i.Cantidad)
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<VentaResponseDto?> GetByIdAsync(int id)
    {
        var venta = await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.MetodoPagoNavigation)
            .Include(v => v.ItemsVenta!).ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(v => v.IdVenta == id);

        if (venta == null) return null;

        return new VentaResponseDto
        {
            IdVenta = venta.IdVenta,
            IdCliente = venta.IdCliente,
            ClienteNombre = venta.Cliente != null
                ? $"{venta.Cliente.Nombres} {venta.Cliente.Apellidos}"
                : null,
            FechaVenta = venta.FechaVenta,
            Total = venta.Total,
            MetodoPago = venta.MetodoPagoNavigation != null
                ? venta.MetodoPagoNavigation.Descripcion
                : venta.MetodoPago,
            Items = venta.ItemsVenta!.Select(i => new ItemVentaResponseDto
            {
                IdItem = i.IdItem,
                IdProducto = i.IdProducto,
                NombreProducto = i.Producto != null ? i.Producto.Nombre : null,
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioUnitario,
                Subtotal = i.Subtotal ?? (i.PrecioUnitario * i.Cantidad)
            }).ToList()
        };
    }

    public async Task<VentaResponseDto> CreateAsync(VentaCreateDto dto)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            // Calcular total y validar stock
            decimal total = 0;
            var items = new List<ItemVenta>();

            foreach (var itemDto in dto.Items)
            {
                var producto = await _db.Productos.FindAsync(itemDto.IdProducto)
                    ?? throw new InvalidOperationException($"Producto con ID {itemDto.IdProducto} no encontrado.");

                if (producto.Stock < itemDto.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {itemDto.Cantidad}.");

                // Descontar stock
                producto.Stock -= itemDto.Cantidad;

                var item = new ItemVenta
                {
                    IdProducto = itemDto.IdProducto,
                    Cantidad = itemDto.Cantidad,
                    PrecioUnitario = producto.PrecioUnitario,
                };
                items.Add(item);
                total += producto.PrecioUnitario * itemDto.Cantidad;
            }

            var venta = new Venta
            {
                IdCliente = dto.IdCliente,
                FechaVenta = DateTime.UtcNow,
                Total = total,
                MetodoPago = dto.MetodoPago,
                ItemsVenta = items
            };

            _db.Ventas.Add(venta);
            await _db.SaveChangesAsync();

            // Generar comprobante automático
            var comprobante = new Comprobante
            {
                IdVenta = venta.IdVenta,
                TipoComprobante = "03", // Boleta
                Serie = "B001",
                Correlativo = await ObtenerSiguienteCorrelativoAsync(),
                FechaEmision = DateTime.UtcNow,
                MontoTotal = total,
                IgvMonto = total * 0.18m, // 18% IGV
                ClienteDocumentoTipo = venta.Cliente?.TipoDocumento,
                ClienteDocumentoNum = venta.Cliente?.Documento,
                ClienteNombre = venta.Cliente != null
                    ? $"{venta.Cliente.Nombres} {venta.Cliente.Apellidos}"
                    : "CLIENTE ANONIMO",
                MetodoPago = dto.MetodoPago,
                IdEstadoSunat = 1
            };
            _db.Comprobantes.Add(comprobante);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation("Venta {Id} creada, total {Total}, comprobante {Serie}-{Correlativo}",
                venta.IdVenta, total, comprobante.Serie, comprobante.Correlativo);

            return await GetByIdAsync(venta.IdVenta)
                ?? throw new InvalidOperationException("Error al recuperar la venta creada.");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var venta = await _db.Ventas
            .Include(v => v.ItemsVenta)
            .FirstOrDefaultAsync(v => v.IdVenta == id);

        if (venta == null) return false;

        // Devolver stock al eliminar una venta
        if (venta.ItemsVenta != null)
        {
            foreach (var item in venta.ItemsVenta)
            {
                var producto = await _db.Productos.FindAsync(item.IdProducto);
                if (producto != null)
                {
                    producto.Stock += item.Cantidad;
                }
            }
        }

        // Eliminar comprobante asociado
        var comprobante = await _db.Comprobantes
            .FirstOrDefaultAsync(c => c.IdVenta == id);
        if (comprobante != null)
        {
            _db.Comprobantes.Remove(comprobante);
        }

        _db.Ventas.Remove(venta);
        await _db.SaveChangesAsync();

        _logger.LogWarning("Venta {Id} eliminada. Stock devuelto.", id);
        return true;
    }

    private async Task<int> ObtenerSiguienteCorrelativoAsync()
    {
        int ultimo = await _db.Comprobantes
            .Where(c => c.Serie == "B001")
            .MaxAsync(c => (int?)c.Correlativo) ?? 0;
        return ultimo + 1;
    }
}

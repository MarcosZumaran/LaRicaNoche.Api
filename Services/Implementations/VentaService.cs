using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class VentaService : IVentaService
{
    private readonly LaRicaNocheDbContext _context;

    public VentaService(LaRicaNocheDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<VentaResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.ItemsVenta)
                .ThenInclude(i => i.Producto)
            .OrderByDescending(v => v.FechaVenta)
            .Take(50)
            .ToListAsync();

        var response = entities.Select(v => MapToDto(v)).ToList();
        return new BaseResponse<List<VentaResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<List<VentaResponseDto>>> GetByFechaAsync(DateTime fecha)
    {
        var entities = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.ItemsVenta)
                .ThenInclude(i => i.Producto)
            .Where(v => v.FechaVenta.Date == fecha.Date)
            .OrderByDescending(v => v.FechaVenta)
            .ToListAsync();

        var response = entities.Select(v => MapToDto(v)).ToList();
        return new BaseResponse<List<VentaResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<VentaResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.ItemsVenta)
                .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(v => v.IdVenta == id);

        if (entity == null) return new BaseResponse<VentaResponseDto> { IsSuccess = false, Message = "Venta no encontrada" };

        return new BaseResponse<VentaResponseDto> { Data = MapToDto(entity) };
    }

    public async Task<BaseResponse<VentaResponseDto>> CreateAsync(CreateVentaDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return new BaseResponse<VentaResponseDto> { IsSuccess = false, Message = "Debe incluir al menos un producto" };

        var total = 0m;
        var items = new List<ItemVenta>();

        foreach (var item in dto.Items)
        {
            var producto = await _context.Productos.FindAsync(item.IdProducto);
            if (producto == null)
                return new BaseResponse<VentaResponseDto> { IsSuccess = false, Message = $"Producto {item.IdProducto} no encontrado" };
            
            if (producto.Stock < item.Cantidad)
                return new BaseResponse<VentaResponseDto> { IsSuccess = false, Message = $"Stock insuficiente para: {producto.Nombre}" };

            producto.Stock -= item.Cantidad;
            total += producto.PrecioVenta * item.Cantidad;

            items.Add(new ItemVenta
            {
                IdProducto = item.IdProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.PrecioVenta
            });
        }

        var venta = new Venta
        {
            IdCliente = dto.IdCliente,
            IdUsuario = dto.IdUsuario,
            MetodoPago = dto.MetodoPago,
            TotalVenta = total,
            ItemsVenta = items
        };

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();

        var ventaCompleta = await _context.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.ItemsVenta)
                .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(v => v.IdVenta == venta.IdVenta);

        return new BaseResponse<VentaResponseDto> { Data = MapToDto(ventaCompleta!) };
    }

    private VentaResponseDto MapToDto(Venta v)
    {
        var itemsDto = v.ItemsVenta?.Select(i => new ItemVentaResponseDto
        {
            IdItem = i.IdItem,
            NombreProducto = i.Producto?.Nombre ?? "Desconocido",
            Cantidad = i.Cantidad,
            PrecioUnitario = i.PrecioUnitario,
            Subtotal = i.Cantidad * i.PrecioUnitario
        }).ToList() ?? new List<ItemVentaResponseDto>();

        return new VentaResponseDto
        {
            IdVenta = v.IdVenta,
            NombreCliente = v.Cliente != null ? v.Cliente.Nombres + " " + v.Cliente.Apellidos : "Sin cliente",
            Usuario = v.IdUsuario.ToString(),
            FechaVenta = v.FechaVenta,
            TotalVenta = v.TotalVenta,
            MetodoPago = v.MetodoPago,
            Items = itemsDto
        };
    }
}
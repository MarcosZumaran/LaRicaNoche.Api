using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;
using LaRicaNoche.Api.Services.Interfaces;
using LaRicaNoche.Api.Services.Interfaces.lua;

namespace LaRicaNoche.Api.Services.Implementations;

public class ProductoService : IProductoService
{
    private readonly LaRicaNocheDbContext _context;
    private readonly ILuaService _luaService;

    public ProductoService(LaRicaNocheDbContext context, ILuaService luaService)
    {
        _context = context;
        _luaService = luaService;
    }

    public async Task<BaseResponse<List<ProductoResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Productos
            .Include(p => p.Categoria)
            .ToListAsync();
            
        var response = entities.Select(p => new ProductoResponseDto
        {
            IdProducto = p.IdProducto,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.Nombre,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
            UnidadMedida = p.UnidadMedida,
            TieneStock = p.Stock > 0
        }).ToList();

        return new BaseResponse<List<ProductoResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<ProductoResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.IdProducto == id);
            
        if (entity == null) return new BaseResponse<ProductoResponseDto> { IsSuccess = false, Message = "Producto no encontrado" };

        var response = new ProductoResponseDto
        {
            IdProducto = entity.IdProducto,
            IdCategoria = entity.IdCategoria,
            NombreCategoria = entity.Categoria?.Nombre,
            Nombre = entity.Nombre,
            PrecioVenta = entity.PrecioVenta,
            Stock = entity.Stock,
            StockMinimo = entity.StockMinimo,
            UnidadMedida = entity.UnidadMedida,
            TieneStock = entity.Stock > 0
        };

        return new BaseResponse<ProductoResponseDto> { Data = response };
    }

    public async Task<BaseResponse<List<ProductoResponseDto>>> GetByCategoriaAsync(int idCategoria)
    {
        var entities = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.IdCategoria == idCategoria)
            .ToListAsync();
            
        var response = entities.Select(p => new ProductoResponseDto
        {
            IdProducto = p.IdProducto,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.Nombre,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
            UnidadMedida = p.UnidadMedida,
            TieneStock = p.Stock > 0
        }).ToList();

        return new BaseResponse<List<ProductoResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<List<ProductoResponseDto>>> GetConStockAsync()
    {
        var entities = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Stock > 0)
            .ToListAsync();
            
        var response = entities.Select(p => new ProductoResponseDto
        {
            IdProducto = p.IdProducto,
            IdCategoria = p.IdCategoria,
            NombreCategoria = p.Categoria?.Nombre,
            Nombre = p.Nombre,
            PrecioVenta = p.PrecioVenta,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
            UnidadMedida = p.UnidadMedida,
            TieneStock = p.Stock > 0
        }).ToList();

        return new BaseResponse<List<ProductoResponseDto>> { Data = response };
    }

    public async Task<BaseResponse<ProductoResponseDto>> CreateAsync(CreateProductoDto dto)
    {
        var categoria = await _context.Categorias.FindAsync(dto.IdCategoria);
        if (categoria == null) return new BaseResponse<ProductoResponseDto> { IsSuccess = false, Message = "Categoría no encontrada" };

        // VALIDACIÓN CON LUA
        var luaResult = _luaService.ExecuteScriptFile("validar_producto.lua", "precio", dto.PrecioVenta, 0);
        if (!(bool)luaResult[0]) 
            return new BaseResponse<ProductoResponseDto> { IsSuccess = false, Message = luaResult[1].ToString() };
        
        // VALIDAR STOCK
        var luaStockResult = _luaService.ExecuteScriptFile("validar_producto.lua", "stock", dto.Stock, dto.StockMinimo);
        if (!(bool)luaStockResult[0]) 
            return new BaseResponse<ProductoResponseDto> { IsSuccess = false, Message = luaStockResult[1].ToString() };
        
        var entity = dto.Adapt<Producto>();
        _context.Productos.Add(entity);
        await _context.SaveChangesAsync();
        
        var response = new ProductoResponseDto
        {
            IdProducto = entity.IdProducto,
            IdCategoria = entity.IdCategoria,
            NombreCategoria = categoria.Nombre,
            Nombre = entity.Nombre,
            PrecioVenta = entity.PrecioVenta,
            Stock = entity.Stock,
            StockMinimo = entity.StockMinimo,
            UnidadMedida = entity.UnidadMedida,
            TieneStock = entity.Stock > 0
        };
        
        return new BaseResponse<ProductoResponseDto> { Data = response };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(int id, UpdateProductoDto dto)
    {
        var entity = await _context.Productos.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Producto no encontrado" };
        
        var categoria = await _context.Categorias.FindAsync(dto.IdCategoria);
        if (categoria == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Categoría no encontrada" };
        
        dto.Adapt(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _context.Productos.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Producto no encontrado" };
        
        _context.Productos.Remove(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }

    public async Task<BaseResponse<bool>> AjustarStockAsync(int id, int cantidad)
    {
        var entity = await _context.Productos.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Producto no encontrado" };
        
        entity.Stock += cantidad;
        if (entity.Stock < 0) return new BaseResponse<bool> { IsSuccess = false, Message = "Stock insuficiente" };
        
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true, Message = $"Stock actualizado: {entity.Stock}" };
    }
}
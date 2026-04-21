using Microsoft.EntityFrameworkCore;
using Mapster;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class CategoriaService : ICategoriaService
{
    private readonly LaRicaNocheDbContext _context;

    public CategoriaService(LaRicaNocheDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<CategoriaResponseDto>>> GetAllAsync()
    {
        var entities = await _context.Categorias.ToListAsync();
        return new BaseResponse<List<CategoriaResponseDto>> { Data = entities.Adapt<List<CategoriaResponseDto>>() };
    }

    public async Task<BaseResponse<CategoriaResponseDto>> GetByIdAsync(int id)
    {
        var entity = await _context.Categorias.FindAsync(id);
        if (entity == null) return new BaseResponse<CategoriaResponseDto> { IsSuccess = false, Message = "Categoría no encontrada" };
        return new BaseResponse<CategoriaResponseDto> { Data = entity.Adapt<CategoriaResponseDto>() };
    }

    public async Task<BaseResponse<CategoriaResponseDto>> CreateAsync(CreateCategoriaDto dto)
    {
        var entity = dto.Adapt<Categoria>();
        _context.Categorias.Add(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<CategoriaResponseDto> { Data = entity.Adapt<CategoriaResponseDto>() };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(int id, CreateCategoriaDto dto)
    {
        var entity = await _context.Categorias.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Categoría no encontrada" };
        
        dto.Adapt(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _context.Categorias.FindAsync(id);
        if (entity == null) return new BaseResponse<bool> { IsSuccess = false, Message = "Categoría no encontrada" };
        
        _context.Categorias.Remove(entity);
        await _context.SaveChangesAsync();
        return new BaseResponse<bool> { Data = true };
    }
}
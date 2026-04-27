using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Request.Create;
using LaRicaNoche.Api.DTOs.Request.Update;
using LaRicaNoche.Api.DTOs.Response;
using LaRicaNoche.Api.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LaRicaNoche.Api.Services.Implementations;

public class TipoHabitacionService : ITipoHabitacionService
{
    private readonly LaRicaNocheDbContext _db;

    public TipoHabitacionService(LaRicaNocheDbContext db) => _db = db;

    public async Task<List<TipoHabitacionResponseDto>> GetAllAsync()
    {
        return await _db.TiposHabitacions
            .ProjectToType<TipoHabitacionResponseDto>()
            .ToListAsync();
    }

    public async Task<TipoHabitacionResponseDto?> GetByIdAsync(int id)
    {
        var entity = await _db.TiposHabitacions.FindAsync(id);
        return entity?.Adapt<TipoHabitacionResponseDto>();
    }

    public async Task<TipoHabitacionResponseDto> CreateAsync(CreateTipoHabitacionDto dto)
    {
        var entity = dto.Adapt<Models.TiposHabitacion>();
        _db.TiposHabitacions.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Adapt<TipoHabitacionResponseDto>();
    }

    public async Task<TipoHabitacionResponseDto?> UpdateAsync(int id, UpdateTipoHabitacionDto dto)
    {
        var entity = await _db.TiposHabitacions.FindAsync(id);
        if (entity == null) return null;

        dto.Adapt(entity); // actualiza propiedades en la entidad existente
        await _db.SaveChangesAsync();
        return entity.Adapt<TipoHabitacionResponseDto>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.TiposHabitacions.FindAsync(id);
        if (entity == null) return false;

        _db.TiposHabitacions.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
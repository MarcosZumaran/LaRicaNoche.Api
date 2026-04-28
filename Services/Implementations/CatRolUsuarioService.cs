using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using LaRicaNoche.Api.Mappings;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class CatRolUsuarioService : ICatRolUsuarioService
{
    private readonly LaRicaNocheDbContext _db;
    private readonly CatRolUsuarioMapper _mapper;

    public CatRolUsuarioService(LaRicaNocheDbContext db, CatRolUsuarioMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatRolUsuarioResponseDto>> GetAllAsync()
    {
        var entities = await _db.CatRolUsuarios.AsNoTracking().ToListAsync();
        return entities.Select(_mapper.ToResponse);
    }

    public async Task<CatRolUsuarioResponseDto?> GetByIdAsync(int id)
    {
        var entity = await _db.CatRolUsuarios.FindAsync(id);
        return entity is not null ? _mapper.ToResponse(entity) : null;
    }

    public async Task<CatRolUsuarioResponseDto> CreateAsync(CatRolUsuarioCreateDto dto)
    {
        var entity = _mapper.FromCreate(dto);
        _db.CatRolUsuarios.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.ToResponse(entity);
    }

    public async Task<bool> UpdateAsync(int id, CatRolUsuarioUpdateDto dto)
    {
        var entity = await _db.CatRolUsuarios.FindAsync(id);
        if (entity is null) return false;
        _mapper.UpdateFromDto(dto, entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.CatRolUsuarios.FindAsync(id);
        if (entity is null) return false;
        _db.CatRolUsuarios.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
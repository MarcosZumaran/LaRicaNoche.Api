using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using LaRicaNoche.Api.Mappings;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class CatAfectacionIgvService : ICatAfectacionIgvService
{
    private readonly LaRicaNocheDbContext _db;
    private readonly CatAfectacionIgvMapper _mapper;

    public CatAfectacionIgvService(LaRicaNocheDbContext db, CatAfectacionIgvMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatAfectacionIgvResponseDto>> GetAllAsync()
    {
        var entities = await _db.CatAfectacionIgvs.AsNoTracking().ToListAsync();
        return entities.Select(_mapper.ToResponse);
    }

    public async Task<CatAfectacionIgvResponseDto?> GetByIdAsync(string codigo)
    {
        var entity = await _db.CatAfectacionIgvs.FindAsync(codigo);
        return entity is not null ? _mapper.ToResponse(entity) : null;
    }

    public async Task<CatAfectacionIgvResponseDto> CreateAsync(CatAfectacionIgvCreateDto dto)
    {
        var entity = _mapper.FromCreate(dto);
        _db.CatAfectacionIgvs.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.ToResponse(entity);
    }

    public async Task<bool> UpdateAsync(string codigo, CatAfectacionIgvUpdateDto dto)
    {
        var entity = await _db.CatAfectacionIgvs.FindAsync(codigo);
        if (entity is null) return false;
        _mapper.UpdateFromDto(dto, entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string codigo)
    {
        var entity = await _db.CatAfectacionIgvs.FindAsync(codigo);
        if (entity is null) return false;
        _db.CatAfectacionIgvs.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
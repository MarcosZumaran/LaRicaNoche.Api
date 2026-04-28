using Microsoft.EntityFrameworkCore;
using LaRicaNoche.Api.Data;
using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;
using LaRicaNoche.Api.Mappings;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Services.Implementations;

public class CatMetodoPagoService : ICatMetodoPagoService
{
    private readonly LaRicaNocheDbContext _db;
    private readonly CatMetodoPagoMapper _mapper;

    public CatMetodoPagoService(LaRicaNocheDbContext db, CatMetodoPagoMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CatMetodoPagoResponseDto>> GetAllAsync()
    {
        var entities = await _db.CatMetodoPagos.AsNoTracking().ToListAsync();
        return entities.Select(_mapper.ToResponse);
    }

    public async Task<CatMetodoPagoResponseDto?> GetByIdAsync(string codigo)
    {
        var entity = await _db.CatMetodoPagos.FindAsync(codigo);
        return entity is not null ? _mapper.ToResponse(entity) : null;
    }

    public async Task<CatMetodoPagoResponseDto> CreateAsync(CatMetodoPagoCreateDto dto)
    {
        var entity = _mapper.FromCreate(dto);
        _db.CatMetodoPagos.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.ToResponse(entity);
    }

    public async Task<bool> UpdateAsync(string codigo, CatMetodoPagoUpdateDto dto)
    {
        var entity = await _db.CatMetodoPagos.FindAsync(codigo);
        if (entity is null) return false;
        _mapper.UpdateFromDto(dto, entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string codigo)
    {
        var entity = await _db.CatMetodoPagos.FindAsync(codigo);
        if (entity is null) return false;
        _db.CatMetodoPagos.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
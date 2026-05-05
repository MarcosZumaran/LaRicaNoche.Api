using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICatTipoComprobanteService
{
    Task<IEnumerable<CatTipoComprobanteResponseDto>> GetAllAsync();
    Task<CatTipoComprobanteResponseDto?> GetByIdAsync(string codigo);
    Task<CatTipoComprobanteResponseDto> CreateAsync(CatTipoComprobanteCreateDto dto);
    Task<bool> UpdateAsync(string codigo, CatTipoComprobanteUpdateDto dto);
    Task<bool> DeleteAsync(string codigo);
}
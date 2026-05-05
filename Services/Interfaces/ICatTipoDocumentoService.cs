using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICatTipoDocumentoService
{
    Task<IEnumerable<CatTipoDocumentoResponseDto>> GetAllAsync();
    Task<CatTipoDocumentoResponseDto?> GetByIdAsync(string codigo);
    Task<CatTipoDocumentoResponseDto> CreateAsync(CatTipoDocumentoCreateDto dto);
    Task<bool> UpdateAsync(string codigo, CatTipoDocumentoUpdateDto dto);
    Task<bool> DeleteAsync(string codigo);
}
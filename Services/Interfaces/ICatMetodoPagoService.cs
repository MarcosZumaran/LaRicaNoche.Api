using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICatMetodoPagoService
{
    Task<IEnumerable<CatMetodoPagoResponseDto>> GetAllAsync();
    Task<CatMetodoPagoResponseDto?> GetByIdAsync(string codigo);
    Task<CatMetodoPagoResponseDto> CreateAsync(CatMetodoPagoCreateDto dto);
    Task<bool> UpdateAsync(string codigo, CatMetodoPagoUpdateDto dto);
    Task<bool> DeleteAsync(string codigo);
}
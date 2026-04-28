using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface ICatEstadoSunatService
{
    Task<IEnumerable<CatEstadoSunatResponseDto>> GetAllAsync();
    Task<CatEstadoSunatResponseDto?> GetByIdAsync(int codigo);
    Task<CatEstadoSunatResponseDto> CreateAsync(CatEstadoSunatCreateDto dto);
    Task<bool> UpdateAsync(int codigo, CatEstadoSunatUpdateDto dto);
    Task<bool> DeleteAsync(int codigo);
}
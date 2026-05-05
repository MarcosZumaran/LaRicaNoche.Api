using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface ICatAfectacionIgvService
{
    Task<IEnumerable<CatAfectacionIgvResponseDto>> GetAllAsync();
    Task<CatAfectacionIgvResponseDto?> GetByIdAsync(string codigo);
    Task<CatAfectacionIgvResponseDto> CreateAsync(CatAfectacionIgvCreateDto dto);
    Task<bool> UpdateAsync(string codigo, CatAfectacionIgvUpdateDto dto);
    Task<bool> DeleteAsync(string codigo);
}
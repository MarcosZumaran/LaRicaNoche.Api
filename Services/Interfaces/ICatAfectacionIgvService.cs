using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface ICatAfectacionIgvService
{
    Task<IEnumerable<CatAfectacionIgvResponseDto>> GetAllAsync();
    Task<CatAfectacionIgvResponseDto?> GetByIdAsync(string codigo);
    Task<CatAfectacionIgvResponseDto> CreateAsync(CatAfectacionIgvCreateDto dto);
    Task<bool> UpdateAsync(string codigo, CatAfectacionIgvUpdateDto dto);
    Task<bool> DeleteAsync(string codigo);
}
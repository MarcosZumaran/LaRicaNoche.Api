using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface ICatRolUsuarioService
{
    Task<IEnumerable<CatRolUsuarioResponseDto>> GetAllAsync();
    Task<CatRolUsuarioResponseDto?> GetByIdAsync(int id);
    Task<CatRolUsuarioResponseDto> CreateAsync(CatRolUsuarioCreateDto dto);
    Task<bool> UpdateAsync(int id, CatRolUsuarioUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
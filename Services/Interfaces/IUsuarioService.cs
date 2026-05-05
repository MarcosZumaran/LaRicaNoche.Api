using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(int id);
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
}
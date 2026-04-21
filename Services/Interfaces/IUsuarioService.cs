using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IUsuarioService
{
    Task<BaseResponse<List<UsuarioResponseDto>>> GetAllAsync();
    Task<BaseResponse<UsuarioResponseDto>> GetByIdAsync(int id);
    Task<BaseResponse<UsuarioResponseDto>> RegisterAsync(CreateUsuarioDto dto);
    Task<BaseResponse<UsuarioResponseDto>> LoginAsync(string username, string password);
}

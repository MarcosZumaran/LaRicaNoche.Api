using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Response;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IEstanciaService
{
    Task<IEnumerable<EstanciaResponseDto>> GetAllAsync();
    Task<EstanciaResponseDto?> GetByIdAsync(int id);
    Task<EstanciaResponseDto> CheckInAsync(CheckInDto dto, int? idUsuario);
    Task<EstanciaResponseDto> CheckOutAsync(int idEstancia, int? idUsuario);
}
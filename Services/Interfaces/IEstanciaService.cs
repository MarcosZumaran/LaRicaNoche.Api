using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IEstanciaService
{
    Task<IEnumerable<EstanciaResponseDto>> GetAllAsync();
    Task<EstanciaResponseDto?> GetByIdAsync(int id);
    Task<EstanciaResponseDto> CheckInAsync(CheckInDto dto, int? idUsuario);
    Task<EstanciaResponseDto> CheckOutAsync(int idEstancia, int? idUsuario);
}
using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IReservaService
{
    Task<List<ReservaResponseDto>> GetAllAsync();
}

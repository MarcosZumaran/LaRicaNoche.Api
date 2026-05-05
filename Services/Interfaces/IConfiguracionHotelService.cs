using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IConfiguracionHotelService
{
    Task<ConfiguracionHotelResponseDto> GetConfiguracionAsync();
}
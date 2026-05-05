using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces
{
    public interface ICierreCajaEnvioService
    {
        Task<CierreCajaEnvioDto> GetEstadoAsync(DateOnly fecha);
        Task<bool> MarcarComoEnviadoAsync(DateOnly fecha);
    }
}
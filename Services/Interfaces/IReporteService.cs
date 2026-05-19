using HotelGenericoApi.DTOs.Response;
using HotelGenericoApi.Models;

namespace HotelGenericoApi.Services.Interfaces
{
    public interface IReporteService
    {
        Task<List<VCierreCajaDiario>> GetCierreCajaAsync(DateOnly fecha);
        Task<List<VEstadoHabitacion>> GetEstadoHabitacionesAsync();
        Task<List<VOcupacionDiaria>> GetOcupacionDiariaAsync(DateOnly fecha);
        Task<List<TopProductoDto>> GetTopProductosAsync(int dias);
    }
}

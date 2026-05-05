using HotelGenericoApi.DTOs.Response;

namespace HotelGenericoApi.Services.Interfaces;

public interface IReporteService
{
    Task<IEnumerable<CierreCajaResponseDto>> GetCierreCajaAsync(DateOnly? fecha);
    Task<IEnumerable<EstadoHabitacionResponseDto>> GetEstadoHabitacionesAsync();
}
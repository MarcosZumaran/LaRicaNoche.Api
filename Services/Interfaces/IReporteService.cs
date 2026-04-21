using LaRicaNoche.Api.DTOs.Base;

namespace LaRicaNoche.Api.Services.Interfaces;

public interface IReporteService
{
    Task<BaseResponse<DashboardDto>> GetDashboardAsync();
    Task<BaseResponse<List<IngresosporFechaDto>>> GetIngresosPorRangoAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<BaseResponse<List<IngresosMensualDto>>> GetIngresosMensualesAsync(int anio);
    Task<BaseResponse<List<ProductoVendidoDto>>> GetProductosMasVendidosAsync(int top = 10);
    Task<BaseResponse<List<OcupacionHabitacionDto>>> GetOcupacionPorPisoAsync();
    Task<BaseResponse<List<DTOs.Base.ProductoResponseDto>>> GetAlertasStockAsync();
    Task<BaseResponse<List<ReporteCajaDto>>> GetCierreCajaAsync(DateTime? fechaInicio, DateTime? fechaFin);
    Task<BaseResponse<List<ResumenCajaDto>>> GetResumenCajaAsync(DateTime? fechaInicio, DateTime? fechaFin);
}
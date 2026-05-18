using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("authenticated")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ReporteController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReporteController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    /// <summary>Obtiene el cierre de caja diario con detalle de ingresos y egresos.</summary>
    /// <param name="fecha">Fecha del cierre (yyyy-MM-dd).</param>
    [HttpGet("cierre-caja")]
    [ProducesResponseType(typeof(List<VCierreCajaDiario>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VCierreCajaDiario>>> GetCierreCaja([FromQuery] DateOnly fecha)
    {
        var result = await _reporteService.GetCierreCajaAsync(fecha);
        return Ok(result);
    }

    /// <summary>Obtiene el estado actual de todas las habitaciones.</summary>
    [HttpGet("estado-habitaciones")]
    [ProducesResponseType(typeof(List<VEstadoHabitacion>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VEstadoHabitacion>>> GetEstadoHabitaciones()
    {
        var result = await _reporteService.GetEstadoHabitacionesAsync();
        return Ok(result);
    }

    /// <summary>Obtiene el reporte de ocupación diaria.</summary>
    /// <param name="fecha">Fecha del reporte (yyyy-MM-dd).</param>
    [HttpGet("ocupacion-diaria")]
    [ProducesResponseType(typeof(List<VOcupacionDiaria>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VOcupacionDiaria>>> GetOcupacionDiaria([FromQuery] DateOnly fecha)
    {
        var result = await _reporteService.GetOcupacionDiariaAsync(fecha);
        return Ok(result);
    }
}

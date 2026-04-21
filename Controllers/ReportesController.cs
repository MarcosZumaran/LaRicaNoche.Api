using Microsoft.AspNetCore.Mvc;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _service;

    public ReportesController(IReporteService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _service.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("ingresos")]
    public async Task<IActionResult> GetIngresosPorRango([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var result = await _service.GetIngresosPorRangoAsync(fechaInicio, fechaFin);
        return Ok(result);
    }

    [HttpGet("ingresos/{anio}")]
    public async Task<IActionResult> GetIngresosMensuales(int anio)
    {
        var result = await _service.GetIngresosMensualesAsync(anio);
        return Ok(result);
    }

    [HttpGet("productos")]
    public async Task<IActionResult> GetProductosMasVendidos([FromQuery] int top = 10)
    {
        var result = await _service.GetProductosMasVendidosAsync(top);
        return Ok(result);
    }

    [HttpGet("ocupacion")]
    public async Task<IActionResult> GetOcupacionPorPiso()
    {
        var result = await _service.GetOcupacionPorPisoAsync();
        return Ok(result);
    }

    [HttpGet("alertas-stock")]
    public async Task<IActionResult> GetAlertasStock()
    {
        var result = await _service.GetAlertasStockAsync();
        return Ok(result);
    }

    [HttpGet("cierre-caja")]
    public async Task<IActionResult> GetCierreCaja([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        var result = await _service.GetCierreCajaAsync(fechaInicio, fechaFin);
        return Ok(result);
    }

    [HttpGet("cierre-caja/resumen")]
    public async Task<IActionResult> GetResumenCaja([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        var result = await _service.GetResumenCajaAsync(fechaInicio, fechaFin);
        return Ok(result);
    }
}
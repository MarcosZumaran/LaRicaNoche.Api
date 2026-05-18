using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using HotelGenericoApi.Models;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("global")]
public class VentaController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentaController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    /// <summary>Obtiene todas las ventas registradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Venta>>> GetAll()
    {
        var ventas = await _ventaService.GetAllAsync();
        return Ok(ventas);
    }

    /// <summary>Obtiene una venta por su ID con ítems y producto.</summary>
    /// <param name="id">ID de la venta.</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Venta>> GetById(int id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        if (venta == null)
            return NotFound();
        return Ok(venta);
    }

    /// <summary>Registra una nueva venta con sus ítems.</summary>
    /// <param name="venta">Datos de la venta incluyendo ítems.</param>
    [HttpPost]
    public async Task<ActionResult<Venta>> Create([FromBody] Venta venta)
    {
        var result = await _ventaService.CreateAsync(venta);
        return CreatedAtAction(nameof(GetById), new { id = result.IdVenta }, result);
    }

    /// <summary>Elimina una venta por su ID.</summary>
    /// <param name="id">ID de la venta.</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _ventaService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}

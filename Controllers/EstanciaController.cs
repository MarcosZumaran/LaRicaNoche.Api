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
public class EstanciaController : ControllerBase
{
    private readonly IEstanciaService _estanciaService;

    public EstanciaController(IEstanciaService estanciaService)
    {
        _estanciaService = estanciaService;
    }

    /// <summary>Obtiene todas las estancias registradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Estancia>>> GetAll()
    {
        var estancias = await _estanciaService.GetAllAsync();
        return Ok(estancias);
    }

    /// <summary>Obtiene una estancia por su ID con detalles completos.</summary>
    /// <param name="id">ID de la estancia.</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Estancia>> GetById(int id)
    {
        var estancia = await _estanciaService.GetByIdAsync(id);
        if (estancia == null)
            return NotFound();
        return Ok(estancia);
    }

    /// <summary>Registra un check-in creando una nueva estancia y marcando la habitación como ocupada.</summary>
    /// <param name="estancia">Datos de la estancia.</param>
    [HttpPost]
    public async Task<ActionResult<Estancia>> Create([FromBody] Estancia estancia)
    {
        var result = await _estanciaService.CreateAsync(estancia);
        return CreatedAtAction(nameof(GetById), new { id = result.IdEstancia }, result);
    }

    /// <summary>Registra el check-out de una estancia, liberando la habitación a limpieza.</summary>
    /// <param name="idEstancia">ID de la estancia.</param>
    /// <param name="idUsuario">ID del usuario que realiza el checkout.</param>
    [HttpPost("{idEstancia}/checkout")]
    public async Task<ActionResult<Estancia>> Checkout(int idEstancia, [FromQuery] int idUsuario)
    {
        var result = await _estanciaService.CheckoutAsync(idEstancia, idUsuario);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    /// <summary>Añade un huésped adicional a una estancia existente.</summary>
    /// <param name="idEstancia">ID de la estancia.</param>
    /// <param name="huesped">Datos del huésped.</param>
    [HttpPost("{idEstancia}/huesped")]
    public async Task<ActionResult> AddHuesped(int idEstancia, [FromBody] Huesped huesped)
    {
        var result = await _estanciaService.AddHuespedAsync(idEstancia, huesped);
        if (!result)
            return BadRequest();
        return Ok();
    }

    /// <summary>Registra un consumo (producto) en una estancia activa.</summary>
    /// <param name="idEstancia">ID de la estancia.</param>
    /// <param name="item">Detalle del consumo (producto, cantidad, precio).</param>
    [HttpPost("{idEstancia}/consumo")]
    public async Task<ActionResult> AddConsumo(int idEstancia, [FromBody] ItemEstancia item)
    {
        var result = await _estanciaService.AddConsumoAsync(idEstancia, item);
        if (!result)
            return BadRequest();
        return Ok();
    }
}

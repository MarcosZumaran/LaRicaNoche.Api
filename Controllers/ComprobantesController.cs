using Microsoft.AspNetCore.Mvc;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprobantesController : ControllerBase
{
    private readonly IComprobanteService _service;

    public ComprobantesController(IComprobanteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("fecha/{fecha}")]
    public async Task<IActionResult> GetByFecha(DateTime fecha)
    {
        var result = await _service.GetByFechaAsync(fecha);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("referencia/{idReferencia}")]
    public async Task<IActionResult> GetByReferencia(int idReferencia, [FromQuery] string tipo = "Reserva")
    {
        var result = await _service.GetByReferenciaAsync(idReferencia, tipo);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComprobanteDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdComprobante }, result);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> AlternarEstado(int id)
    {
        var result = await _service.AlternarEstadoAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }
}
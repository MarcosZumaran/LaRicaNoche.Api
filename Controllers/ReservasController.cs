using Microsoft.AspNetCore.Mvc;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.Services.Interfaces;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _service;

    public ReservasController(IReservaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservaDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.IdReserva }, result);
    }

    [HttpPut("{id}/checkout")]
    public async Task<IActionResult> Checkout(int id)
    {
        var result = await _service.FinalizarReservaAsync(id);
        if (!result.IsSuccess) return NotFound(result);
        return Ok(result);
    }
}

using LaRicaNoche.Api.DTOs.Request;
using LaRicaNoche.Api.DTOs.Request.Create;
using LaRicaNoche.Api.DTOs.Request.Update;
using LaRicaNoche.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitacionesController : ControllerBase
{
    private readonly IHabitacionService _service;
    public HabitacionesController(IHabitacionService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        return dto == null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHabitacionDto dto)
    {
        var response = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = response.IdHabitacion }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHabitacionDto dto)
    {
        var response = await _service.UpdateAsync(id, dto);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoHabitacionDto dto)
    {
        var resultado = await _service.CambiarEstadoAsync(id, dto);
        if (!resultado) return BadRequest(new { mensaje = "No se puede realizar esa transición de estado según las reglas del negocio." });

        var habitacion = await _service.GetByIdAsync(id);
        return Ok(habitacion);
    }

    [HttpGet("estado/{nombre}")]
    public async Task<IActionResult> GetPorEstado(string nombre)
    {
        var lista = await _service.GetHabitacionesPorEstadoAsync(nombre);
        return Ok(lista);
    }
}
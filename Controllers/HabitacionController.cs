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
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class HabitacionController : ControllerBase
{
    private readonly IHabitacionService _habitacionService;

    public HabitacionController(IHabitacionService habitacionService)
    {
        _habitacionService = habitacionService;
    }

    /// <summary>Obtiene todas las habitaciones registradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<Habitacion>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Habitacion>>> GetAll()
    {
        var habitaciones = await _habitacionService.GetAllAsync();
        return Ok(habitaciones);
    }

    /// <summary>Obtiene una habitación por su ID.</summary>
    /// <param name="id">ID de la habitación.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Habitacion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Habitacion>> GetById(int id)
    {
        var habitacion = await _habitacionService.GetByIdAsync(id);
        if (habitacion == null)
            return NotFound();
        return Ok(habitacion);
    }

    /// <summary>Crea una nueva habitación.</summary>
    /// <param name="habitacion">Datos de la habitación.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Habitacion), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Habitacion>> Create([FromBody] Habitacion habitacion)
    {
        var result = await _habitacionService.CreateAsync(habitacion);
        return CreatedAtAction(nameof(GetById), new { id = result.IdHabitacion }, result);
    }

    /// <summary>Actualiza los datos de una habitación existente.</summary>
    /// <param name="id">ID de la habitación.</param>
    /// <param name="habitacionActualizada">Datos actualizados.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Habitacion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Habitacion>> Update(int id, [FromBody] Habitacion habitacionActualizada)
    {
        var result = await _habitacionService.UpdateAsync(id, habitacionActualizada);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    /// <summary>Elimina una habitación por su ID.</summary>
    /// <param name="id">ID de la habitación.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _habitacionService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    /// <summary>Cambia el estado de una habitación validando transiciones permitidas.</summary>
    /// <param name="idHabitacion">ID de la habitación.</param>
    /// <param name="idNuevoEstado">ID del nuevo estado.</param>
    /// <param name="idUsuario">ID del usuario que realiza el cambio.</param>
    /// <param name="observacion">Observación opcional.</param>
    [HttpPatch("{idHabitacion}/estado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CambiarEstado(int idHabitacion, [FromQuery] int idNuevoEstado, [FromQuery] int idUsuario, [FromQuery] string? observacion = null)
    {
        var result = await _habitacionService.CambiarEstadoAsync(idHabitacion, idNuevoEstado, idUsuario, observacion);
        if (!result)
            return NotFound();
        return NoContent();
    }
}

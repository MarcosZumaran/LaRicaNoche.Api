using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HabitacionController : ControllerBase
{
    private readonly IHabitacionService _service;

    public HabitacionController(IHabitacionService service)
    {
        _service = service;
    }

    private int? ObtenerIdUsuario()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null) return null;
        return int.TryParse(claim.Value, out int id) ? id : null;
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
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(HabitacionCreateDto dto)
    {
        try
        {
            var idUsuario = ObtenerIdUsuario();
            var result = await _service.CreateAsync(dto, idUsuario);
            return CreatedAtAction(nameof(GetById), new { id = result.IdHabitacion }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, HabitacionUpdateDto dto)
    {
        try
        {
            var idUsuario = ObtenerIdUsuario();
            var updated = await _service.UpdateAsync(id, dto, idUsuario);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, HabitacionUpdateDto dto)
    {
        try
        {
            var idUsuario = ObtenerIdUsuario();
            var updated = await _service.UpdateAsync(id, dto, idUsuario);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpGet("estado-actual")]
    public async Task<IActionResult> GetEstadoActual()
    {
        var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
        var result = await _service.GetEstadoActualAsync(rolUsuario);
        return Ok(result);
    }
}
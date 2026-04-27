using LaRicaNoche.Api.DTOs.Request.Create;
using LaRicaNoche.Api.DTOs.Request.Update;
using LaRicaNoche.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LaRicaNoche.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiposHabitacionController : ControllerBase
{
    private readonly ITipoHabitacionService _service;

    public TiposHabitacionController(ITipoHabitacionService service) => _service = service;

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
    public async Task<IActionResult> Create([FromBody] CreateTipoHabitacionDto dto)
    {
        var response = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = response.IdTipo }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTipoHabitacionDto dto)
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
}
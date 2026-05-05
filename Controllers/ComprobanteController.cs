using Microsoft.AspNetCore.Mvc;
using HotelGenericoApi.Services.Interfaces;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprobanteController : ControllerBase
{
    private readonly IComprobanteService _service;

    public ComprobanteController(IComprobanteService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost("{id}/enviar")]
    public async Task<IActionResult> MarcarEnviado(int id, [FromBody] string hashXml)
    {
        var updated = await _service.MarcarComoEnviadoAsync(id, hashXml);
        return updated ? NoContent() : NotFound();
    }
}
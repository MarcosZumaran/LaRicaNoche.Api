using Microsoft.AspNetCore.Mvc;
using HotelGenericoApi.DTOs.Request;
using HotelGenericoApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelGenericoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductoController(IProductoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>Crea un producto nuevo, opcionalmente con imagen.</summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] ProductoCreateDto dto, IFormFile? file)
    {
        var result = await _service.CreateAsync(dto, file);
        return CreatedAtAction(nameof(GetById), new { id = result.IdProducto }, result);
    }

    /// <summary>Actualiza un producto existente, opcionalmente con nueva imagen.</summary>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductoUpdateDto dto, IFormFile? file)
    {
        var updated = await _service.UpdateAsync(id, dto, file);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>Agrega stock a un producto existente.</summary>
    [HttpPost("{id}/entrada-stock")]
    public async Task<IActionResult> AddStock(int id, [FromBody] EntradaStockDto dto)
    {
        try
        {
            var result = await _service.AddStockAsync(id, dto.Cantidad);
            return result ? NoContent() : NotFound(new { mensaje = "Producto no encontrado." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}

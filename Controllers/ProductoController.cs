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

    [HttpPost]
    public async Task<IActionResult> Create(ProductoCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.IdProducto }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductoUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>Agrega stock a un producto existente.</summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="dto">Cantidad a agregar.</param>
    [HttpPost("{id}/entrada-stock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>Sube una imagen para un producto y actualiza su URL.</summary>
    [HttpPost("{id}/imagen")]
    public async Task<IActionResult> SubirImagen(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { mensaje = "No se recibió ninguna imagen." });

        // Validar que sea una imagen
        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!extensionesPermitidas.Contains(extension))
            return BadRequest(new { mensaje = "Formato de imagen no permitido. Use JPG, PNG, GIF o WebP." });

        // Generar nombre único
        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaRelativa = Path.Combine("imagenes", "productos", nombreArchivo);
        var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaRelativa);

        // Crear carpeta si no existe (por si acaso)
        Directory.CreateDirectory(Path.GetDirectoryName(rutaCompleta)!);

        // Guardar archivo
        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Actualizar la URL en la base de datos
        var urlRelativa = $"/{rutaRelativa.Replace(Path.DirectorySeparatorChar, '/')}";
        await _service.SetImagenUrlAsync(id, urlRelativa);

        return Ok(new { imagenUrl = urlRelativa });
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
}

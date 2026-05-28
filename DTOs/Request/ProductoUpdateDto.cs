using Microsoft.AspNetCore.Mvc;

namespace HotelGenericoApi.DTOs.Request;

public sealed record ProductoUpdateDto
{
    [FromForm] public string? CodigoSunat { get; init; }
    [FromForm] public string? Nombre { get; init; }
    [FromForm] public string? Descripcion { get; init; }
    [FromForm] public decimal? PrecioUnitario { get; init; }
    [FromForm] public string? IdAfectacionIgv { get; init; }
    [FromForm] public int? IdCategoria { get; init; }
    [FromForm] public int? Stock { get; init; }
    [FromForm] public int? StockMinimo { get; init; }
    [FromForm] public string? UnidadMedida { get; init; }
    [FromForm] public IFormFile? File { get; init; }
}

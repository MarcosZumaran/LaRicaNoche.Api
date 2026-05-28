using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HotelGenericoApi.DTOs.Request;

public sealed record ProductoCreateDto
{
    [FromForm, StringLength(10)]
    public string? CodigoSunat { get; init; }

    [FromForm, Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(200, MinimumLength = 2)]
    public string Nombre { get; init; } = string.Empty;

    [FromForm, StringLength(500)]
    public string? Descripcion { get; init; }

    [FromForm, Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
    public decimal PrecioUnitario { get; init; }

    [FromForm, StringLength(2)]
    public string? IdAfectacionIgv { get; init; } = "10";

    [FromForm, Range(0, int.MaxValue)]
    public int? IdCategoria { get; init; }

    [FromForm, Range(0, int.MaxValue)]
    public int? Stock { get; init; }

    [FromForm, Range(0, int.MaxValue)]
    public int? StockMinimo { get; init; }

    [FromForm, StringLength(10)]
    public string? UnidadMedida { get; init; } = "NIU";
}

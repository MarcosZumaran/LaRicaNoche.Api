using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

public sealed record ProductoCreateDto
{
    [StringLength(10)]
    public string? CodigoSunat { get; init; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(200, MinimumLength = 2)]
    public string Nombre { get; init; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; init; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
    public decimal PrecioUnitario { get; init; }

    [StringLength(2)]
    public string? IdAfectacionIgv { get; init; } = "10";

    [Range(0, int.MaxValue)]
    public int? Stock { get; init; }

    [Range(0, int.MaxValue)]
    public int? StockMinimo { get; init; }

    [StringLength(10)]
    public string? UnidadMedida { get; init; } = "NIU";

    public string? ImagenUrl { get; init; }
}

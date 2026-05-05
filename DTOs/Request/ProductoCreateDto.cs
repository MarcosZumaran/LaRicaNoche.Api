namespace HotelGenericoApi.DTOs.Request;

public sealed record ProductoCreateDto
{
    public string? CodigoSunat { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public decimal PrecioUnitario { get; init; }
    public string? IdAfectacionIgv { get; init; } = "10";
    public int? Stock { get; init; }
    public int? StockMinimo { get; init; }
    public string? UnidadMedida { get; init; } = "NIU";
}
namespace LaRicaNoche.Api.DTOs.Request;

public sealed record ProductoUpdateDto
{
    public string? CodigoSunat { get; init; }
    public string? Nombre { get; init; }
    public string? Descripcion { get; init; }
    public decimal? PrecioUnitario { get; init; }
    public string? IdAfectacionIgv { get; init; }
    public int? Stock { get; init; }
    public int? StockMinimo { get; init; }
    public string? UnidadMedida { get; init; }
}
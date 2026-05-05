namespace HotelGenericoApi.DTOs.Response;

public sealed record ProductoResponseDto(
    int IdProducto,
    string? CodigoSunat,
    string Nombre,
    string? Descripcion,
    decimal PrecioUnitario,
    string? IdAfectacionIgv,
    string? NombreAfectacionIgv,
    int? Stock,
    int? StockMinimo,
    string? UnidadMedida,
    DateTime? CreatedAt
);
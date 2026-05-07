namespace HotelGenericoApi.DTOs.Response;

public sealed record ItemConsumoResponseDto(
    int IdItem,
    int IdProducto,
    string NombreProducto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal,
    DateTime? FechaRegistro
);
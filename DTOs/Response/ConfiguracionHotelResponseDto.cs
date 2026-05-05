namespace HotelGenericoApi.DTOs.Response;

public sealed record ConfiguracionHotelResponseDto(
    string Nombre,
    string? Direccion,
    string? Telefono,
    string? Ruc,
    decimal TasaIgvHotel,
    decimal TasaIgvProductos
);
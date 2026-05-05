namespace HotelGenericoApi.DTOs.Response;

public sealed record TiposHabitacionResponseDto(
    int IdTipo,
    string Nombre,
    int Capacidad,
    string? Descripcion,
    decimal PrecioBase
);
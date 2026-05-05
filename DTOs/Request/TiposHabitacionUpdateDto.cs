namespace HotelGenericoApi.DTOs.Request;

public sealed record TiposHabitacionUpdateDto
{
    public string? Nombre { get; init; }
    public int? Capacidad { get; init; }
    public string? Descripcion { get; init; }
    public decimal? PrecioBase { get; init; }
}
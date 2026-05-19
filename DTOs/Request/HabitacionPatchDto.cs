namespace HotelGenericoApi.DTOs.Request;

public sealed record HabitacionPatchDto
{
    public int? IdEstado { get; init; }
    public string? NumeroHabitacion { get; init; }
    public int? Piso { get; init; }
    public string? Descripcion { get; init; }
    public int? IdTipo { get; init; }
    public decimal? PrecioNoche { get; init; }
}

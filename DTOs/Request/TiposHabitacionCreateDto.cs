namespace LaRicaNoche.Api.DTOs.Request;

public sealed record TiposHabitacionCreateDto
{
    public string Nombre { get; init; } = string.Empty;
    public int Capacidad { get; init; } = 2;
    public string? Descripcion { get; init; }
    public decimal PrecioBase { get; init; } = 50.00m;
}
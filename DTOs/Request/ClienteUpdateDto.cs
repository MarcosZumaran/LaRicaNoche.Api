namespace HotelGenericoApi.DTOs.Request;

public sealed record ClienteUpdateDto
{
    public string? Nombres { get; init; }
    public string? Apellidos { get; init; }
    public string? Nacionalidad { get; init; }
    public DateOnly? FechaNacimiento { get; init; }
    public string? Telefono { get; init; }
    public string? Email { get; init; }
    public string? Direccion { get; init; }
}
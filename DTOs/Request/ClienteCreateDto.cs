namespace LaRicaNoche.Api.DTOs.Request;

public sealed record ClienteCreateDto
{
    public string TipoDocumento { get; init; } = "1";   // 1 = DNI por defecto
    public string Documento { get; init; } = string.Empty;
    public string Nombres { get; init; } = string.Empty;
    public string Apellidos { get; init; } = string.Empty;
    public string? Nacionalidad { get; init; } = "PERUANA";
    public DateOnly? FechaNacimiento { get; init; }
    public string? Telefono { get; init; }
    public string? Email { get; init; }
    public string? Direccion { get; init; }
}
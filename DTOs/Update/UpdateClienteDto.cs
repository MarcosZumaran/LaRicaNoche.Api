namespace LaRicaNoche.Api.DTOs.Update;

public record UpdateClienteDto(
    string? TipoDocumento,
    string? Documento,
    string? Nombres,
    string? Apellidos,
    string? Telefono,
    string? Email,
    string? Nacionalidad,
    string? Direccion = null
);

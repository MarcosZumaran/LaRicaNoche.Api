namespace LaRicaNoche.Api.DTOs.Base;

public record ClienteResponseDto(
    int IdCliente,
    string? TipoDocumento,
    string Documento,
    string? Nombres,
    string? Apellidos,
    string? Nacionalidad,
    DateTime? FechaNacimiento,
    string? Telefono,
    string? Email,
    string? Direccion,
    DateTime FechaRegistro
);

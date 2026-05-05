namespace HotelGenericoApi.DTOs.Response;

public sealed record ClienteResponseDto(
    int IdCliente,
    string TipoDocumento,
    string Documento,
    string Nombres,
    string Apellidos,
    string? Nacionalidad,
    DateOnly? FechaNacimiento,
    string? Telefono,
    string? Email,
    string? Direccion,
    DateTime FechaRegistro,
    DateTime? FechaVerificacionReniec
);
namespace HotelGenericoApi.DTOs.Response;

/// <summary>Datos de respuesta de un cliente.</summary>
/// <param name="IdCliente">Identificador único del cliente.</param>
/// <param name="TipoDocumento">Tipo de documento (1=DNI, 6=RUC).</param>
/// <param name="Documento">Número de documento.</param>
/// <param name="Nombres">Nombres del cliente.</param>
/// <param name="Apellidos">Apellidos del cliente.</param>
/// <param name="Nacionalidad">Nacionalidad.</param>
/// <param name="FechaNacimiento">Fecha de nacimiento.</param>
/// <param name="Telefono">Teléfono de contacto.</param>
/// <param name="Email">Correo electrónico.</param>
/// <param name="Direccion">Dirección de domicilio.</param>
/// <param name="FechaRegistro">Fecha de registro en el sistema.</param>
/// <param name="FechaVerificacionReniec">Fecha de última verificación en RENIEC.</param>
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

namespace HotelGenericoApi.DTOs.Request;

/// <summary>Datos para crear un nuevo cliente.</summary>
public sealed record ClienteCreateDto
{
    /// <summary>Tipo de documento (1=DNI, 6=RUC, etc.).</summary>
    public string TipoDocumento { get; init; } = "1";

    /// <summary>Número de documento.</summary>
    public string Documento { get; init; } = string.Empty;

    /// <summary>Nombres del cliente.</summary>
    public string Nombres { get; init; } = string.Empty;

    /// <summary>Apellidos del cliente.</summary>
    public string Apellidos { get; init; } = string.Empty;

    /// <summary>Nacionalidad.</summary>
    public string? Nacionalidad { get; init; } = "PERUANA";

    /// <summary>Fecha de nacimiento.</summary>
    public DateOnly? FechaNacimiento { get; init; }

    /// <summary>Teléfono de contacto.</summary>
    public string? Telefono { get; init; }

    /// <summary>Correo electrónico.</summary>
    public string? Email { get; init; }

    /// <summary>Dirección de domicilio.</summary>
    public string? Direccion { get; init; }
}

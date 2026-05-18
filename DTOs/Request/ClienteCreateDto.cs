using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

/// <summary>Datos para crear un nuevo cliente.</summary>
public sealed record ClienteCreateDto
{
    /// <summary>Tipo de documento (1=DNI, 6=RUC, etc.).</summary>
    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    [StringLength(2, MinimumLength = 1)]
    public string TipoDocumento { get; init; } = "1";

    /// <summary>Número de documento.</summary>
    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [StringLength(15, MinimumLength = 6)]
    public string Documento { get; init; } = string.Empty;

    /// <summary>Nombres del cliente.</summary>
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombres { get; init; } = string.Empty;

    /// <summary>Apellidos del cliente.</summary>
    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2)]
    public string Apellidos { get; init; } = string.Empty;

    /// <summary>Nacionalidad.</summary>
    [StringLength(50)]
    public string? Nacionalidad { get; init; } = "PERUANA";

    /// <summary>Fecha de nacimiento.</summary>
    public DateOnly? FechaNacimiento { get; init; }

    /// <summary>Teléfono de contacto.</summary>
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [StringLength(20)]
    public string? Telefono { get; init; }

    /// <summary>Correo electrónico.</summary>
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [StringLength(100)]
    public string? Email { get; init; }

    /// <summary>Dirección de domicilio.</summary>
    [StringLength(200)]
    public string? Direccion { get; init; }
}

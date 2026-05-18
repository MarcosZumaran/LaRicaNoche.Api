using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

/// <summary>Credenciales de inicio de sesión.</summary>
public sealed record LoginDto
{
    /// <summary>Nombre de usuario.</summary>
    [Required]
    public string Username { get; init; } = string.Empty;

    /// <summary>Contraseña.</summary>
    [Required]
    public string Password { get; init; } = string.Empty;
}

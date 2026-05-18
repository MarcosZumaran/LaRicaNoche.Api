using System.ComponentModel.DataAnnotations;

namespace HotelGenericoApi.DTOs.Request;

public sealed record UsuarioCreateDto
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int IdRol { get; init; }
}
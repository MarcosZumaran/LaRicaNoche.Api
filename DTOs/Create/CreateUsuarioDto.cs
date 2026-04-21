using System.ComponentModel.DataAnnotations;

namespace LaRicaNoche.Api.DTOs.Create;

public record CreateUsuarioDto(
    [Required] string Username,
    [Required, MinLength(6)] string Password,
    [Required] string Rol
);

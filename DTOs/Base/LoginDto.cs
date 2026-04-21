using System.ComponentModel.DataAnnotations;

namespace LaRicaNoche.Api.DTOs.Base;

public record LoginDto(
    [Required] string Username,
    [Required] string Password
);

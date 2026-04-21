namespace LaRicaNoche.Api.DTOs.Update;

public record UpdateUsuarioDto(
    string? Username,
    string? Rol,
    bool? EstaActivo
);

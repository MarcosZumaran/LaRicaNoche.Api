namespace LaRicaNoche.Api.DTOs.Base;

public record UsuarioResponseDto(
    int IdUsuario,
    string Username,
    string PasswordHash,
    string Rol,
    DateTime FechaCreacion,
    bool EstaActivo
);

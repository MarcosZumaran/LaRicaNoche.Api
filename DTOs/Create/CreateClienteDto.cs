using System.ComponentModel.DataAnnotations;

namespace LaRicaNoche.Api.DTOs.Create;

public record CreateClienteDto(
    [Required] string TipoDocumento,
    [Required] string Documento,
    string? Nombres = null,
    string? Apellidos = null,
    string? Nacionalidad = "Peruana",
    DateTime? FechaNacimiento = null,
    string? Telefono = null,
    string? Email = null,
    string? Direccion = null
);

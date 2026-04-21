using System.ComponentModel.DataAnnotations;

namespace LaRicaNoche.Api.DTOs.Create;

public record CreateHabitacionDto(
    [Required] string NumeroHabitacion,
    int Piso,
    decimal PrecioNoche
);

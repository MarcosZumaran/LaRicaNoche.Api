namespace LaRicaNoche.Api.DTOs.Update;

public record UpdateHabitacionDto(
    string? NumeroHabitacion,
    decimal? PrecioNoche,
    string? Estado
);

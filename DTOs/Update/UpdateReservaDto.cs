namespace LaRicaNoche.Api.DTOs.Update;

public record UpdateReservaDto(
    DateTime? FechaEntrada,
    DateTime? FechaSalida,
    decimal? MontoTotal,
    string? MetodoPago,
    string? EstadoReserva, // Activa, Finalizada, Cancelada
    string? NumBoleta
);

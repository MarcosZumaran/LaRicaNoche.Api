namespace LaRicaNoche.Api.DTOs.Base;

public record ReservaResponseDto(
    int IdReserva,
    int IdCliente,
    string? NombreCliente,
    int IdHabitacion,
    string? NumeroHabitacion,
    DateTime FechaRegistro,
    DateTime FechaEntrada,
    DateTime FechaSalida,
    decimal MontoTotal,
    string? MetodoPago,
    string? EstadoReserva,
    string? NumBoleta
);

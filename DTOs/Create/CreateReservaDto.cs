using System.ComponentModel.DataAnnotations;

namespace LaRicaNoche.Api.DTOs.Create;

public record CreateReservaDto(
    [Required] int IdCliente,
    [Required] int IdHabitacion,
    [Required] int IdUsuarioRecepcion,
    [Required] DateTime FechaEntrada,
    [Required] DateTime FechaSalida,
    [Required] decimal MontoTotal,
    string? MetodoPago = "Efectivo",
    string? NumBoleta = null
);

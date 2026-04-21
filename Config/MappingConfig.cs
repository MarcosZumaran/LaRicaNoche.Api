using Mapster;
using LaRicaNoche.Api.Models;
using LaRicaNoche.Api.DTOs.Base;
using LaRicaNoche.Api.DTOs.Create;
using LaRicaNoche.Api.DTOs.Update;

namespace LaRicaNoche.Api.Config;

public static class MappingConfig
{
    public static void Configure()
    {
        // 1. Usuario -> DTO (Nunca enviamos el PasswordHash)
        TypeAdapterConfig<Usuario, UsuarioResponseDto>.NewConfig()
            .Map(dest => dest.IdUsuario, src => src.IdUsuario);

        // 2. Reserva -> DTO (Mapeamos nombres de Clientes y Habitaciones)
        TypeAdapterConfig<Reserva, ReservaResponseDto>.NewConfig()
            .Map(dest => dest.NombreCliente, src => src.Cliente != null ? src.Cliente.Nombres + " " + src.Cliente.Apellidos : "Sin Cliente")
            .Map(dest => dest.NumeroHabitacion, src => src.Habitacion != null ? src.Habitacion.NumeroHabitacion : "Sin Nro");

        // 3. Habitacion -> DTO
        TypeAdapterConfig<Habitacion, HabitacionResponseDto>.NewConfig()
            .Map(dest => dest.IdHabitacion, src => src.IdHabitacion)
            .Map(dest => dest.NumeroHabitacion, src => src.NumeroHabitacion)
            .Map(dest => dest.Piso, src => src.Piso)
            .Map(dest => dest.PrecioNoche, src => src.PrecioNoche)
            .Map(dest => dest.Estado, src => src.Estado)
            .Map(dest => dest.FechaUltimoCheckout, src => src.FechaUltimoCheckout);
    }
}

namespace LaRicaNoche.Api.DTOs.Response;

public class HabitacionResponseDTO
{
    public int IdHabitacion { get; set; }
    public string NumeroHabitacion { get; set; } = string.Empty;
    public int? Piso { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioNoche { get; set; }

    // Info tipo habitación
    public string TipoHabitacionNombre { get; set; } = string.Empty;
    public int? TipoHabitacionCapacidad { get; set; }


    // Info estado
    public string EstadoActual { get; set; } = string.Empty;
    public DateTime? FechaUltimoCambio { get; set; }
}
namespace HotelGenericoApi.Models;

public class VEstadoHabitacion
{
    public string? NumeroHabitacion { get; set; }
    public string? TipoHabitacion { get; set; }
    public string? Estado { get; set; }
    public decimal PrecioNoche { get; set; }
    public DateTime? FechaUltimoCambio { get; set; }
}
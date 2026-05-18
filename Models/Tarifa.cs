namespace HotelGenericoApi.Models;

public class Tarifa
{
    public int IdTarifa { get; set; }
    public int IdTipoHabitacion { get; set; }
    public int? IdTemporada { get; set; }
    public decimal Precio { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }

    // Navegación
    public TipoHabitacion? TipoHabitacion { get; set; }
    public Temporada? Temporada { get; set; }
}
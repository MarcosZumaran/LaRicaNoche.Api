namespace HotelGenericoApi.Models;

public class TipoHabitacion
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = null!;
    public int? Capacidad { get; set; }
    public string? Descripcion { get; set; }
    public decimal? PrecioBase { get; set; }
}
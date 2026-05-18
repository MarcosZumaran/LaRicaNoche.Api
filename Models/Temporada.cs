namespace HotelGenericoApi.Models;

public class Temporada
{
    public int IdTemporada { get; set; }
    public string Nombre { get; set; } = null!;
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public decimal Multiplicador { get; set; } = 1.00m;
}
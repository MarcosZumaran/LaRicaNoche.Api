namespace HotelGenericoApi.Models;

public class VOcupacionDiaria
{
    public DateOnly Fecha { get; set; }
    public int Ocupadas { get; set; }
    public int Total { get; set; }
    public decimal PorcentajeOcupacion { get; set; }
}
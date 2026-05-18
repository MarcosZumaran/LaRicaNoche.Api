namespace HotelGenericoApi.Models;

public class Huesped
{
    public int IdHuesped { get; set; }
    public int IdEstancia { get; set; }
    public int IdCliente { get; set; }
    public bool? EsTitular { get; set; }
    public DateTime? FechaRegistro { get; set; }

    // Navegación
    public Estancia? Estancia { get; set; }
    public Cliente? Cliente { get; set; }
}
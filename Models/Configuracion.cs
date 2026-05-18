namespace HotelGenericoApi.Models;

public class Configuracion
{
    public int IdConfiguracion { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Ruc { get; set; }
    public decimal TasaIgvHotel { get; set; } = 18.00m;
    public decimal TasaIgvProductos { get; set; } = 18.00m;
}
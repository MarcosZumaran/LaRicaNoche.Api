namespace HotelGenericoApi.Models;

public class EstadoSunat
{
    public int Codigo { get; set; }
    public string Descripcion { get; set; } = null!;
    public string? DescripcionLarga { get; set; }
}
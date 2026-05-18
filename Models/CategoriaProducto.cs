namespace HotelGenericoApi.Models;

public class CategoriaProducto
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}
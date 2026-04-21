namespace LaRicaNoche.Api.DTOs.Base;

public class ProductoResponseDto
{
    public int IdProducto { get; set; }
    public int IdCategoria { get; set; }
    public string? NombreCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = "NIU";
    public bool TieneStock { get; set; }
}
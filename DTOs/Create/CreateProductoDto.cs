namespace LaRicaNoche.Api.DTOs.Create;

public class CreateProductoDto
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; } = 5;
    public string UnidadMedida { get; set; } = "NIU";
}
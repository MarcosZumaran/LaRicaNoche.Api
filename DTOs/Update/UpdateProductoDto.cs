namespace LaRicaNoche.Api.DTOs.Update;

public class UpdateProductoDto
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = "NIU";
}
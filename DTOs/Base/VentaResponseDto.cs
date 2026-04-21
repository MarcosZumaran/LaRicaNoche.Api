namespace LaRicaNoche.Api.DTOs.Base;

public class VentaResponseDto
{
    public int IdVenta { get; set; }
    public string? NombreCliente { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaVenta { get; set; }
    public decimal TotalVenta { get; set; }
    public string MetodoPago { get; set; } = "Efectivo";
    public List<ItemVentaResponseDto> Items { get; set; } = new();
}

public class ItemVentaResponseDto
{
    public int IdItem { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
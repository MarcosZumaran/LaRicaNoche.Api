namespace HotelGenericoApi.Models;

public class ItemVenta
{
    public int IdItem { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; } // columna computada

    // Navegación
    public Venta? Venta { get; set; }
    public Producto? Producto { get; set; }
}
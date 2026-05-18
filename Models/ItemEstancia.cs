namespace HotelGenericoApi.Models;

public class ItemEstancia
{
    public int IdItem { get; set; }
    public int IdEstancia { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; } // columna computada
    public DateTime? FechaRegistro { get; set; }

    // Navegación
    public Estancia? Estancia { get; set; }
    public Producto? Producto { get; set; }
}
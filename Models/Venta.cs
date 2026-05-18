namespace HotelGenericoApi.Models;

public class Venta
{
    public int IdVenta { get; set; }
    public int? IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FechaVenta { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; } = null!; // FK

    // Navegación
    public Cliente? Cliente { get; set; }
    public Usuario? Usuario { get; set; }
    public MetodoPago? MetodoPagoNavigation { get; set; }
    public ICollection<ItemVenta> ItemsVenta { get; set; } = new List<ItemVenta>();
}
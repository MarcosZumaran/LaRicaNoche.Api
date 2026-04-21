using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("items_venta")]
public class ItemVenta
{
    [Key]
    [Column("id_item")]
    public int IdItem { get; set; }

    [Column("id_venta")]
    public int IdVenta { get; set; }

    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unitario")]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey("IdVenta")]
    public Venta? Venta { get; set; }

    [ForeignKey("IdProducto")]
    public Producto? Producto { get; set; }
}
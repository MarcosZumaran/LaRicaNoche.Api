using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("productos")]
public class Producto
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Required, MaxLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("precio_venta")]
    public decimal PrecioVenta { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("stock_minimo")]
    public int StockMinimo { get; set; } = 5;

    [MaxLength(3)]
    [Column("unidad_medida")]
    public string? UnidadMedida { get; set; }

    [ForeignKey("IdCategoria")]
    public Categoria? Categoria { get; set; }
}
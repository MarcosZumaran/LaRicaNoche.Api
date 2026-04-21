using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("ventas")]
public class Venta
{
    [Key]
    [Column("id_venta")]
    public int IdVenta { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("fecha_venta")]
    public DateTime FechaVenta { get; set; } = DateTime.Now;

    [Column("total_venta")]
    public decimal TotalVenta { get; set; }

    [MaxLength(20)]
    [Column("metodo_pago")]
    public string? MetodoPago { get; set; }

    [ForeignKey("IdCliente")]
    public Cliente? Cliente { get; set; }

    public List<ItemVenta> ItemsVenta { get; set; } = new();
}
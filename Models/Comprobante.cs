using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaRicaNoche.Api.Models;

[Table("comprobantes")]
public class Comprobante
{
    [Key]
    [Column("id_comprobante")]
    public int IdComprobante { get; set; }

    [Column("id_referencia")]
    public int IdReferencia { get; set; }

    [MaxLength(20)]
    [Column("tipo_referencia")]
    public string? TipoReferencia { get; set; }

    [MaxLength(20)]
    [Column("tipo_comprobante")]
    public string? TipoComprobante { get; set; }

    [MaxLength(4)]
    [Column("serie")]
    public string? Serie { get; set; }

    [Column("correlativo")]
    public int Correlativo { get; set; }

    [Column("fecha_emision")]
    public DateTime? FechaEmision { get; set; }

    [Column("monto_total")]
    public decimal MontoTotal { get; set; }

    [MaxLength(20)]
    [Column("cliente_documento")]
    public string? ClienteDocumento { get; set; }

    [MaxLength(100)]
    [Column("cliente_nombres")]
    public string? ClienteNombres { get; set; }

    [MaxLength(20)]
    [Column("metodo_pago")]
    public string? MetodoPago { get; set; }

    [MaxLength(20)]
    [Column("estado_sunat")]
    public string? EstadoSunat { get; set; }
}
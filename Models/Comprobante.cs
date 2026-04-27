using System;
using System.Collections.Generic;

namespace LaRicaNoche.Api.Models;

public partial class Comprobante
{
    public int IdComprobante { get; set; }

    public int? IdEstancia { get; set; }

    public int? IdVenta { get; set; }

    public string? TipoComprobante { get; set; }

    public string Serie { get; set; } = null!;

    public int Correlativo { get; set; }

    public DateTime? FechaEmision { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal IgvMonto { get; set; }

    public string? ClienteDocumentoTipo { get; set; }

    public string? ClienteDocumentoNum { get; set; }

    public string? ClienteNombre { get; set; }

    public string? MetodoPago { get; set; }

    public string? EstadoSunat { get; set; }

    public string? XmlFirmado { get; set; }

    public byte[]? CdrZip { get; set; }

    public virtual CatTipoDocumento? ClienteDocumentoTipoNavigation { get; set; }

    public virtual CatMetodoPago? MetodoPagoNavigation { get; set; }

    public virtual CatTipoComprobante? TipoComprobanteNavigation { get; set; }
}

namespace HotelGenericoApi.Models;

public class Comprobante
{
    public int IdComprobante { get; set; }
    public int? IdEstancia { get; set; }
    public int? IdVenta { get; set; }
    public string TipoComprobante { get; set; } = null!; // FK
    public string Serie { get; set; } = null!;
    public int Correlativo { get; set; }
    public DateTime? FechaEmision { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal IgvMonto { get; set; }
    public string? ClienteDocumentoTipo { get; set; }
    public string? ClienteDocumentoNum { get; set; }
    public string? ClienteNombre { get; set; }
    public string? MetodoPago { get; set; } // FK
    public int IdEstadoSunat { get; set; }
    public string? XmlFirmado { get; set; }
    public byte[]? CdrZip { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public int? IntentosEnvio { get; set; }
    public string? HashXml { get; set; }

    // Navegación
    public Estancia? Estancia { get; set; }
    public Venta? Venta { get; set; }
    public TipoComprobante? TipoComprobanteNavigation { get; set; }
    public TipoDocumento? ClienteDocumentoTipoNavigation { get; set; }
    public MetodoPago? MetodoPagoNavigation { get; set; }
    public EstadoSunat? EstadoSunat { get; set; }
}
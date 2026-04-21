namespace LaRicaNoche.Api.DTOs.Base;

public class ComprobanteResponseDto
{
    public int IdComprobante { get; set; }
    public int IdReferencia { get; set; }
    public string TipoReferencia { get; set; } = string.Empty;
    public string TipoComprobante { get; set; } = "Boleta";
    public string Serie { get; set; } = "B001";
    public string Correlativo { get; set; } = string.Empty;
    public string NumeroCompleto { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public string ClienteDocumento { get; set; } = string.Empty;
    public string ClienteNombres { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Igv { get; set; }
    public decimal MontoTotal { get; set; }
    public string EstadoSunat { get; set; } = "Pendiente";
}
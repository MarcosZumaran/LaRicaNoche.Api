namespace LaRicaNoche.Api.DTOs.Create;

public class CreateComprobanteDto
{
    public int IdReferencia { get; set; }
    public string TipoReferencia { get; set; } = "Reserva"; // Reserva o Venta
    public string TipoComprobante { get; set; } = "Boleta"; // Boleta o Factura
    public string ClienteDocumento { get; set; } = string.Empty;
    public string ClienteNombres { get; set; } = string.Empty;
}
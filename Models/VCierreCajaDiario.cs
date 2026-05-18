namespace HotelGenericoApi.Models;

public class VCierreCajaDiario
{
    public DateOnly Fecha { get; set; }
    public string? MetodoPago { get; set; }
    public decimal Ingresos { get; set; }
    public string? Concepto { get; set; }
}
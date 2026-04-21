namespace LaRicaNoche.Api.DTOs.Base;

public class ReporteCajaDto
{
    public DateTime Fecha { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public decimal Ingresos { get; set; }
    public string Concepto { get; set; } = string.Empty;
}

public class ResumenCajaDto
{
    public DateTime Fecha { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Yape { get; set; }
    public decimal Tarjeta { get; set; }
    public decimal Total { get; set; }
}
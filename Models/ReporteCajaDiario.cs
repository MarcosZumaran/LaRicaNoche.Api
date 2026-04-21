using Microsoft.EntityFrameworkCore;

namespace LaRicaNoche.Api.Models;

[Keyless]
public class ReporteCajaDiario
{
    public DateTime Fecha { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public decimal Ingresos { get; set; }
    public string Concepto { get; set; } = string.Empty;
}

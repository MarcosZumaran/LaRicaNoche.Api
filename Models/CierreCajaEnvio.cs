namespace HotelGenericoApi.Models;

public class CierreCajaEnvio
{
    public DateOnly Fecha { get; set; }
    public int IdEstadoSunat { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public int? IntentosEnvio { get; set; }
    public string? HashXml { get; set; }

    // Navegación
    public EstadoSunat? EstadoSunat { get; set; }
}
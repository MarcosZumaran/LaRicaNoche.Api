namespace HotelGenericoApi.Models;

public class EstadoHabitacion
{
    public int IdEstado { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool PermiteCheckin { get; set; }
    public bool PermiteCheckout { get; set; }
    public bool EsEstadoFinal { get; set; }
    public string? ColorUi { get; set; }
}
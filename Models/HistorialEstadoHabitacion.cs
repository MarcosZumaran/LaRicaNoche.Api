namespace HotelGenericoApi.Models;

public class HistorialEstadoHabitacion
{
    public int IdHistorial { get; set; }
    public int IdHabitacion { get; set; }
    public int? IdEstadoAnterior { get; set; }
    public int? IdEstadoNuevo { get; set; }
    public DateTime? FechaCambio { get; set; }
    public int? IdUsuario { get; set; }
    public string? Observacion { get; set; }

    // Navegación
    public Habitacion? Habitacion { get; set; }
    public Usuario? Usuario { get; set; }
}